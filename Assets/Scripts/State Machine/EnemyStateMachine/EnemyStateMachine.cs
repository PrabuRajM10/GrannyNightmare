using Gameplay;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace State_Machine.EnemyStateMachine
{
    public class EnemyStateMachine : MonoBehaviour , IDamageable
    {
        [FormerlySerializedAs("_agent")] [SerializeField] private NavMeshAgent agent;
        [FormerlySerializedAs("_animator")] [SerializeField] private Animator animator; 
        [FormerlySerializedAs("_overlapRadius")] [SerializeField] private float overlapRadius = 5f;

        [SerializeField] EnemyBaseState initialState;
        [SerializeField] private EnemyPatrolHelper enemyPatrolHelper;
        [SerializeField] ZombieHand zombieHand;
        [FormerlySerializedAs("health")] [SerializeField] private int maxHealth = 5;
        [SerializeField] private int currentHealth;
        [SerializeField] private float patrolSpeed = 0.8f;
        [SerializeField] private float chaseSpeed = 1.5f;

        EnemyBaseState currentState;   
        
        private PlayerStateMachine.PlayerStates.PlayerStateMachine player;
        private Vector3 newDestination;
        private int isPatrolingHash;
        private int isChasingHash;
        private int isAttackingHash;
        private int isIdleHash;
        private int isDeadHash;
        private bool isChasing;
        private bool isAttacking;
        private bool canChasePlayer , isPlayerFound;

        public Animator Animator => animator;
        public NavMeshAgent NavAgent => agent;  
        public EnemyPatrolHelper EnemyPatrolHelper => enemyPatrolHelper;

        public PlayerStateMachine.PlayerStates.PlayerStateMachine TargetPlayer
        {
            set => player = value;
            get => player;
        }
        public int IsIdleHash => isIdleHash;
        public int IsPatrolingHash => isPatrolingHash;
        public int IsChasingHash => isChasingHash;
        public int IsAttackingHash => isAttackingHash;
        public int IsDeadHash => isDeadHash;
        
        public float PatrolSpeed => patrolSpeed;
        public float ChaseSpeed => chaseSpeed;
        private void OnValidate()
        {
            if(agent == null)agent = GetComponent<NavMeshAgent>();  
            if(animator == null)animator = GetComponent<Animator>();  
            if(enemyPatrolHelper == null)enemyPatrolHelper = GetComponent<EnemyPatrolHelper>();  
        }


        void Awake()
        {
            isPatrolingHash = Animator.StringToHash("IsPatroling");
            isChasingHash = Animator.StringToHash("IsChasing");
            isAttackingHash = Animator.StringToHash("IsAttacking");
            isDeadHash = Animator.StringToHash("IsDead");
            isIdleHash = Animator.StringToHash("IsIdle");

            currentState = initialState;
            currentHealth = maxHealth;
        }

        private void Start()
        {
            currentState.OnEnter(this);
        }

        private void Update()
        {
            if(currentState!=null)currentState.OnUpdate(this);
        }


        public void SetTargetPlayer(PlayerStateMachine.PlayerStates.PlayerStateMachine playerStateMachine)
        {
            player = playerStateMachine;
        }

        public void SwitchStates(EnemyBaseState targetState)
        {
            currentState.OnExit(this);
            currentState = targetState;
            currentState.OnEnter(this);
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, overlapRadius);   
        }

        public void TakeDamage(int damage)
        {
            currentHealth -= damage;
            currentHealth = Mathf.Max(0, currentHealth);
        }

        public void TurnOffLocomotion(bool state)
        {
            agent.isStopped = state;
        }

        public void LookAtTarget(Transform target)
        {
            transform.LookAt(player.transform);
            // var targetForward = target.TransformDirection(Vector3.forward);
            // var localForward = transform.TransformDirection(Vector3.forward);
            // var direction = (target.position - localForward).normalized;
            // LookAtDirection(direction);
        }

        public void LookAtDirection(Vector3 direction)
        {
            transform.rotation = Quaternion.LookRotation(direction , Vector3.up);    
        }

        public int GetHealth()
        {
            return currentHealth;
        }

        public void EnableAttack(bool state)
        {
            zombieHand.EnableAttack(state);
        }
    }
}

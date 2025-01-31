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

        [SerializeField] EnemyBaseState initialState;
        [SerializeField] private EnemyPatrolHelper enemyPatrolHelper;
        [FormerlySerializedAs("zombieHand")] [SerializeField] EnemyHand enemyHand;
        [FormerlySerializedAs("health")] [SerializeField] private int maxHealth = 5;
        [SerializeField] private int currentHealth;
        [SerializeField] private float patrolSpeed = 0.8f;
        [SerializeField] private float chaseSpeed = 1.5f;
        [FormerlySerializedAs("airChaseSpeed")] [FormerlySerializedAs("airChase")] [SerializeField] private float jumpChaseSpeed = 2f;

        EnemyBaseState currentState;   
        
        private PlayerStateMachine.PlayerStates.PlayerStateMachine player;
        private Vector3 newDestination;
        private int isPatrolingHash;
        private int isChasingHash;
        private int isLightAttackingHash;
        private int isHeavyAttackingHash;
        private int isJumpAttackingHash;
        private int isRangedAttackingHash;
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
        public int IsLightAttackingHash => isLightAttackingHash;
        public int IsHeavyAttackingHash => isHeavyAttackingHash;
        public int IsJumpAttackingHash => isJumpAttackingHash;
        public int IsRangedAttackingHash => isRangedAttackingHash;
        public int IsDeadHash => isDeadHash;
        
        public float PatrolSpeed => patrolSpeed;
        public float ChaseSpeed => chaseSpeed;
        public float JumpChaseSpeed => jumpChaseSpeed;
        public bool CanChasePlayer => canChasePlayer;

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
            isLightAttackingHash = Animator.StringToHash("IsAttacking_Light");
            isHeavyAttackingHash = Animator.StringToHash("IsAttacking_Heavy");
            isJumpAttackingHash = Animator.StringToHash("IsAttacking_Jump");
            isRangedAttackingHash = Animator.StringToHash("IsAttacking_Ranged");
            isDeadHash = Animator.StringToHash("IsDead");
            isIdleHash = Animator.StringToHash("IsIdle");

            currentState = initialState;
            currentHealth = maxHealth;
        }

        private void Start()
        {
            if(currentState != null)currentState.OnEnter(this);
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

        public void TakeDamage(int damage)
        {
            currentHealth -= damage;
            currentHealth = Mathf.Max(0, currentHealth);
        }

        public void TurnOffLocomotion(bool state)
        {
            Debug.Log("[Enemy] [TurnOffLocomotion] " + state);
            agent.isStopped = state;
        }

        public void LookAtTarget(Transform target)
        {
            transform.LookAt(target.transform);
            // var targetForward = target.TransformDirection(Vector3.forward);
            // var localForward = transform.TransformDirection(Vector3.forward);
            // var direction = (target.position - transform.position).normalized;
            // Debug.DrawRay(transform.position, direction, Color.red , 100);
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
            enemyHand.EnableAttack(state);
        }

        public void SetInitialPosition(Transform patrolPointsPoint)
        {
            transform.localPosition = patrolPointsPoint.localPosition;    
        }

        public void SetParent(Transform parent)
        {
            transform.SetParent(parent);
        }
        
        //Animation event 
        public void StartChasing()
        {
            Debug.Log("[Enemy] [StartChasing]");
            canChasePlayer = true;
        }
        public void StopChasing()
        {
            Debug.Log("[Enemy] [StopChasing]");
            canChasePlayer = false;
        }
    }
}

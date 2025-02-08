using System;
using System.Collections;
using Gameplay;
using ObjectPooling;
using Ui;
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
        [SerializeField] Transform projectileSpawnPoint;
        [SerializeField] private Transform initialPosition;
        [SerializeField] UiManager uiManager;
        [SerializeField] PoolManager poolManager;
        [FormerlySerializedAs("health")] [SerializeField] private int maxHealth = 5;
        [SerializeField] private int currentHealth;
        [SerializeField] private float patrolSpeed = 0.8f;
        [SerializeField] private float chaseSpeed = 1.5f;
        [SerializeField] private float baseJumpChaseSpeed = 2f;
        
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
        private bool isAttackDone , isPlayerFound , canBehave;

        public delegate void Callback();
        Callback onCalculateAttackDamage;
        Callback onPlayAudioCallback;

        public Animator Animator => animator;
        public NavMeshAgent NavAgent => agent;  

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

        public int JumpChaseSpeed
        {
            get
            {
                var d = Vector3.Distance(transform.position, TargetPlayer.transform.position);
                var m = 6; // min distance
                d = Mathf.Round(Mathf.Clamp(d, m, 12));
                int x = (int)Mathf.Abs(d - m);

                return (int)(baseJumpChaseSpeed + x + (x + 1) / 2);
            }
        }
        public Transform ProjectileSpawnPoint => projectileSpawnPoint;

        public bool IsAttackDone => isAttackDone;

        private void OnValidate()
        {
            if(agent == null)agent = GetComponent<NavMeshAgent>();  
            if(animator == null)animator = GetComponent<Animator>();  
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
            SetInitialPosition();

        }

        private void Start()
        {
            uiManager.SetUpEnemyHealth(maxHealth);
        }

        private void Update()
        {
            if(canBehave && currentState!=null)currentState.OnUpdate(this);
        }

        public void StartEnemyBehaviour()
        {
            canBehave = true;
            if(currentState != null)currentState.OnEnter(this);
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
            uiManager.UpdateEnemyHealth(currentHealth);
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
        }

        public void LookAtDirection(Vector3 direction)
        {
            transform.rotation = Quaternion.LookRotation(direction , Vector3.up);    
        }

        public int GetHealth()
        {
            return currentHealth;
        }

        private void SetInitialPosition()
        {
            transform.localPosition = initialPosition.localPosition;    
        }

        public void SetParent(Transform parent)
        {
            transform.SetParent(parent);
        }

        public void SubscribeDamageCalculation(Callback damageCalc)
        {
            onCalculateAttackDamage = damageCalc;
        }

        public void SubscribePlayAudio(Callback audioCallback)
        {
            onPlayAudioCallback = audioCallback;
        }
        public void UnSubscribeCallbacks()
        {
            onCalculateAttackDamage = null;
            onPlayAudioCallback = null;
        }

        public void Dead()
        {
            uiManager.SetGameResult(true);
            StartCoroutine(nameof(GameOver));
        }
        
        IEnumerator GameOver()
        {
            yield return new WaitForSeconds(2);
            uiManager.SwitchScreen(GameScreens.GamePlayScreen , GameScreens.GameResult);
        }

        public void Reset()
        {
            currentState.OnExit(this);
            currentState = initialState;
            currentState.OnEnter(this);
            currentHealth = maxHealth;
            uiManager.UpdateEnemyHealth(currentHealth , 0);
            SetInitialPosition();
            canBehave = false;  
        }

        //Animation event 

        public void StartAttack()
        {
            isAttackDone = false;
        }

        public void EndAttack()
        {
            isAttackDone = true;
        }

        public void StartMoving()
        {
            TurnOffLocomotion(false);
        }

        public void StopMoving()
        {
            TurnOffLocomotion(true);
        }

        public void CalculateDamageOnAttack()
        {
            onCalculateAttackDamage?.Invoke();
        }

        public void PlayAudio()
        {
            onPlayAudioCallback?.Invoke();
        }
    }
}

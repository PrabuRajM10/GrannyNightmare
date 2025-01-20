using System;
using System.Collections;
using System.Collections.Generic;
using State_Machine.States.PlayerStates;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace State_Machine.States.EnemyStates
{
    public class EnemyStateMachine : MonoBehaviour
    {
        private Vector3 newDestination;
        [FormerlySerializedAs("_overlapRadius")] [SerializeField] private float overlapRadius = 5f;
        [FormerlySerializedAs("EnemyViewDistance")] [SerializeField] private float enemyViewDistance;
        [FormerlySerializedAs("viewAnlge")] [FormerlySerializedAs("ViewAnlge")] [SerializeField] private float viewAngle;

        [SerializeField] EnemyBaseState initialState;
        [SerializeField] private EnemyPatrolHelper enemyPatrolHelper;
        
        EnemyBaseState currentState;   
        
        private bool canChasePlayer , isPlayerFound;
        private PlayerStateMachine player;
        [FormerlySerializedAs("_agent")] [SerializeField] private NavMeshAgent agent;
        [FormerlySerializedAs("_animator")] [SerializeField] private Animator animator;
        private int isWalkingHash;
        private int isChasingHash;
        private int isAttackingHash;
        private int isIdleHash;
        private int isDeadHash;
        private bool isChasing;
        private bool isAttacking;
        private float patrolSpeed;
        private float chaseSpeed;

        public Animator Animator => animator;
        public NavMeshAgent NavAgent => agent;  
        public EnemyPatrolHelper EnemyPatrolHelper => enemyPatrolHelper;

        public PlayerStateMachine TargetPlayer
        {
            set => player = value;
            get => player;
        }
        public int IsIdleHash => isIdleHash;
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
            isWalkingHash = Animator.StringToHash("IsWalking");
            isChasingHash = Animator.StringToHash("IsChasing");
            isAttackingHash = Animator.StringToHash("IsAttacking");

            currentState = initialState;
        }

        private void Start()
        {
            currentState.OnEnter(this);
        }

        private void Update()
        {
            if(currentState!=null)currentState.OnUpdate(this);
        }
        void LookForPlayer()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, overlapRadius);

            foreach (var hitCollider in hitColliders)
            {
                var playerObj = hitCollider.gameObject.GetComponent<PlayerStateMachine>();  
                if (playerObj != null)
                {
                    var playerObjTransform = playerObj.transform;
                    var dirVect = (playerObjTransform.position - transform.position).normalized;
                    var dotResult = Vector3.Dot(dirVect, transform.forward.normalized);
                    //Debug.Log("HandlePlayerDetection player dot result " + dotResult);

                    if ((Vector3.Angle(transform.forward, dirVect) < viewAngle / 2))
                    {
                        var newPos = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
                        if (Physics.Raycast(newPos, dirVect, enemyViewDistance))
                        {
                            Debug.DrawLine(newPos, playerObjTransform.position, Color.yellow, 1000);
                            Debug.Log("Player got caught");
                            player = playerObj;
                            (isChasing, isPlayerFound, agent.isStopped) = (true,true,true);
                        }
                    }
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, overlapRadius);   
        }
    }
}

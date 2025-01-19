using System;
using System.Collections;
using System.Collections.Generic;
using State_Machine.States.PlayerStates;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace State_Machine.States.EnemyStates
{
    public class EnemyStateMachine : StateMachine
    {
        private Vector3 newDestination;
        [FormerlySerializedAs("_overlapRadius")] [SerializeField] private float overlapRadius = 5f;
        [FormerlySerializedAs("EnemyViewDistance")] [SerializeField] private float enemyViewDistance;
        [FormerlySerializedAs("viewAnlge")] [FormerlySerializedAs("ViewAnlge")] [SerializeField] private float viewAngle;
        [FormerlySerializedAs("PositionList")] [SerializeField] private List<Transform> positionList= new List<Transform>();
        [SerializeField] private float destinationBuffer = 0.1f;

        private bool canChasePlayer , isPlayerFound;
        private PlayerStateMachine player;

        void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _currentPlayerState.OnEnterState();
            _animator = GetComponent<Animator>();
            _isWalkingHash = Animator.StringToHash("IsWalking");
            _isChasingHash = Animator.StringToHash("IsChasing");
            _isAttackingHash = Animator.StringToHash("IsAttacking");
        }

        private void Start()
        {
            newDestination = GetNewDestination(positionList);
            _agent.SetDestination(newDestination);
            restTime = UnityEngine.Random.Range(3, 7);
        }

        private void Update()
        {
            if(_currentPlayerState!=null)_currentPlayerState.OnUpdateState();
            switch (isPlayerFound)
            {
                case false when !canChasePlayer:
                    LookForPlayer();
                    Patrol();
                    break;
                case true when canChasePlayer:
                    newDestination = player.transform.position;
                    _agent.SetDestination(newDestination);
                    _agent.isStopped = false;
                    if (Vector3.Distance(newDestination, transform.position) > destinationBuffer)
                    {
                        if(!_isChasing)_isChasing = true;
                        if(_isAttacking)_isAttacking = false;
                    }
                    else
                    {
                        if(_isChasing)_isAttacking = false;
                        if(!_isAttacking)_isAttacking = true;
                    }
                    break;
            }
        }

        private void Patrol()
        {
            if (Vector3.Distance(newDestination, transform.position) > destinationBuffer)
            {
                if(!_isWalking)_isWalking = true;
            }
            else
            {
                _isWalking = false;
                _isWaiting = true;
                    
                if (restTime > 0f)
                {
                    restTime -= Time.deltaTime;
                }
                else
                {
                    newDestination = GetNewDestination(positionList);
                    _isWalking = true;
                    _agent.SetDestination(newDestination);
                    restTime = UnityEngine.Random.Range(3, 7);
                }

            }
        }

        Vector3 GetNewDestination(List<Transform> transList)
        {
            var count = transList.Count;
            List<Transform> newTransList = new List<Transform>();
            foreach(var trans in transList)
            {
                if (Vector3.Distance(trans.position, transform.position) > 1f) newTransList.Add(trans);    
            }
            
            return newTransList[UnityEngine.Random.Range(0, newTransList.Count)].position;
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
                            (_isChasing, isPlayerFound, _agent.isStopped) = (true,true,true);
                        }
                    }
                }
            }
        }
        public void StartChasing()
        {
            Debug.Log("StartChasing - Animation event");
            canChasePlayer = true;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, overlapRadius);   
        }
    }
}

using System.Collections;
using System.Collections.Generic;
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

        void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _states = new StateMachineHandle(this);
            _currentState = _states.Enemy_Idle();
            _currentState.OnEnterState();
            _animator = GetComponent<Animator>();
            _isWalkingHash = Animator.StringToHash("IsWalking");
        }

        private void Start()
        {
            newDestination = GetNewDestination(positionList);
            _agent.SetDestination(newDestination);
            restTime = UnityEngine.Random.Range(3, 7);
            // StartCoroutine(HandleMovement());
            StartCoroutine(HandlePlayerDetection());

        }

        private void Update()
        {
            if(_currentState!=null)_currentState.OnUpdateState();

            if (Vector3.Distance(newDestination, transform.position) > destinationBuffer)
            {
                if(!_isWalking)_isWalking = true;
            }
            else
            {
                _isWalking = false;
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
        public override void HandleKill()
        {
            base.HandleKill();
        }

        IEnumerator HandleMovement()
        {
            while (true)
            {
                if (Vector3.Distance(newDestination, transform.position) > destinationBuffer)
                {
                    if(!_isWalking)_isWalking = true;
                    // yield return new WaitForSeconds(0.1f);
                }
                else
                {

                    _isWalking = false;
                    if (restTime > 0f)
                    {
                        yield return new WaitForSeconds(1f);
                        restTime--;
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

        IEnumerator HandlePlayerDetection()
        {

            while(true)
            {
                yield return new WaitForSeconds(0.5f);
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, overlapRadius);

                foreach (var hitCollider in hitColliders)
                {
                    if (hitCollider.gameObject.tag == "Player")
                    {
                        var player = hitCollider.transform;
                        var dirVect = (player.position - transform.position).normalized;
                        var dotResult = Vector3.Dot(dirVect, transform.forward.normalized);
                        //Debug.Log("HandlePlayerDetection player dot result " + dotResult);

                        if ((Vector3.Angle(transform.forward, dirVect) < viewAngle / 2))
                        {
                            var newPos = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
                            if (Physics.Raycast(newPos, dirVect, enemyViewDistance))
                            {
                                Debug.DrawLine(newPos, player.position, Color.yellow, 1000);
                                Debug.Log("Player got caught");
                            }
                        }
                    }
                }
            }
        
        }
    }
}

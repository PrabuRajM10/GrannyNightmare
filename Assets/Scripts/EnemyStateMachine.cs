using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : StateMachine
{
    NavMeshAgent _agent;
    Vector3 _newDestination;
    [SerializeField] float _overlapRadius = 5f, EnemyViewDistance , ViewAnlge;
    [SerializeField]List<Transform> PositionList= new List<Transform>();   
    
    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _states = new StateMachineHandle(this);
        _currentState = _states.Enemy_Idle();
        _currentState.OnEnterState();
        animator = GetComponent<Animator>();

        _isWalkinghash = Animator.StringToHash("IsWalking");

    }

    private void Start()
    {
        _newDestination = GetNewDestination(PositionList);
        _agent.SetDestination(_newDestination);
        _restTime = UnityEngine.Random.Range(3, 7);
        StartCoroutine(HandleMovement());
        StartCoroutine(HandlePlayerDetection());

    }

    private void Update()
    {
        if(_currentState!=null)_currentState.OnUpdateState();
    }
    public override void HandleKill()
    {
        base.HandleKill();
    }

    IEnumerator HandleMovement()
    {
        while (true)
        {
            if (Vector3.Distance(_newDestination, transform.position) > 0.5f)
            {
                _isWalking = true;
                yield return new WaitForSeconds(0.1f);
            }
            else
            {

                _isWalking = false;
                if (_restTime > 0f)
                {
                    yield return new WaitForSeconds(1f);
                    _restTime--;
                }
                else
                {
                    _newDestination = GetNewDestination(PositionList);
                    _isWalking = true;
                    _agent.SetDestination(_newDestination);
                    _restTime = UnityEngine.Random.Range(3, 7);

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
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, _overlapRadius);

            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.gameObject.tag == "Player")
                {
                    var player = hitCollider.transform;
                    var dirVect = (player.position - transform.position).normalized;
                    var dotResult = Vector3.Dot(dirVect, transform.forward.normalized);
                    //Debug.Log("HandlePlayerDetection player dot result " + dotResult);

                    if ((Vector3.Angle(transform.forward, dirVect) < ViewAnlge / 2))
                    {
                        var newPos = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
                        if (Physics.Raycast(newPos, dirVect, EnemyViewDistance))
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

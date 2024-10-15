using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PigMove : MoveComponent
{
    [SerializeField] private float _moveRange; // 활동 거리

    public override void OnMove()
    {
        if (NavAgentCompo.isActiveAndEnabled)
        {
            NavAgentCompo.ResetPath();
            Vector3 randomPoint = GetRandomPoint();
            if (randomPoint != Vector3.zero)
            {
                NavAgentCompo.SetDestination(randomPoint);
            }
        }
    }

    private Vector3 GetRandomPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * _moveRange;
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;

        if (NavMesh.SamplePosition(randomDirection, out hit, _moveRange, NavMesh.AllAreas))
        {
            finalPosition = hit.position;
        }

        return finalPosition;
    }

    private bool IsSomethingInFront()
    {
        bool hitSomething = Physics.Raycast(_owner.transform.position, _owner.transform.forward, out RaycastHit hit, 15f);
        return hitSomething;
    }
}

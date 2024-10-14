using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PigMove : MoveComponent
{
    private LayerMask m_layerMask;

    protected override void Awake()
    {
        base.Awake();

        m_layerMask = LayerMask.NameToLayer("Ground");
    }

    public override void OnMove()
    {
        if (NavAgentCompo.isActiveAndEnabled)
        {
            NavAgentCompo.ResetPath();
            NavAgentCompo.SetDestination(GetRandomPoint());
        }
    }

    private Vector3 GetRandomPoint()
    {
        NavMeshHit hit;
        Vector3 randomPoint = Random.insideUnitSphere * 10f;
        randomPoint.y = 1.9f;

        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            Vector3 dir = (hit.position - _owner.transform.position).normalized;
            Vector3 checkPos = _owner.transform.position + (dir * 2f);

            bool isGround = Physics.Raycast(checkPos, Vector3.down, 10f, m_layerMask);
            if (!isGround)
            {
                return hit.position;
            }
        }
        return GetRandomPoint();
    }

    private bool IsSomethingInFront()
    {
        bool hitSomething = Physics.Raycast(_owner.transform.position, _owner.transform.forward, out RaycastHit hit, 2f);
        return hitSomething;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/TransitionCondition/StopCondition")]
public class StopCondition : TransitionCondition
{
    public override bool IsConditionValid()
    {
        return Owner.MoveCompo.NavAgentCompo.remainingDistance <= Owner.MoveCompo.NavAgentCompo.stoppingDistance 
        && !Owner.MoveCompo.NavAgentCompo.pathPending;
    }
}

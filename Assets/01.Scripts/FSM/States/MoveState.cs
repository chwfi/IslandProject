using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State
{
    public MoveState(Entity owner, StateMachine stateMachine, string animName) : base(owner, stateMachine, animName)
    {
    }

    public override void EnterState()
    {
        base.EnterState();

        _owner.MoveCompo.OnMove();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        _owner.IsConditionsValid(StateTypeEnum.Idle);
    }

    public override void ExitState()
    {
        base.ExitState();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorComponent : BaseComponent
{
    public Animator Animator { get; private set; }

    protected virtual void Awake()
    {
        Animator = transform.GetComponent<Animator>();
    }
    
    public void AnimationEndTrigger() // 애니메이션 이벤트에서 달아주는 함수. 애니메이션 마지막 프레임에 실행된다
    {
        _owner.StateMachineCompo.ChangeState(StateTypeEnum.Idle);
    }
}

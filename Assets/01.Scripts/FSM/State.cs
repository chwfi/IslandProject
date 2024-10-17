using UnityEngine;

public class State
{
    protected Entity _owner; // State의 주체
    protected StateMachine _stateMachine;
    protected int _animBoolHash; // 애니메이션 실행을 위한 해쉬

    public State(Entity owner, StateMachine stateMachine, string animName)
    {
        _owner = owner;
        _stateMachine = stateMachine;
        _animBoolHash = Animator.StringToHash(animName);
    }

    public virtual void EnterState() // State의 시작을 알리는 함수
    {
        _owner.AnimatroCompo.Animator.SetBool(_animBoolHash, true);
        // State 시작 시, State의 이름을 따와 그에 맞는 애니메이션을 자동으로 실행시킴
    }

    public virtual void UpdateState() // Update에서 실행해주는 함수
    {

    }

    public virtual void ExitState() // State가 끝날때 실행되는 함수
    {
        _owner.AnimatroCompo.Animator.SetBool(_animBoolHash, false);
        // State 종료 시 애니메이션도 종료
    }
}
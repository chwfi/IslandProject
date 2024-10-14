using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TransitionCondition : ScriptableObject
{
    [SerializeField] private string _description;
    public StateTypeEnum TargetStateType; // 목표 State. 조건이 만족된다면 실행할 State이다.

    public Entity Owner { get; set; }

    public TransitionCondition OnRegister(Entity owner)
    {
        var clone = Instantiate(this);
        clone.Owner = owner;
        
        return clone;
    }

    public abstract bool IsConditionValid(); // 조건이 True인지 False인지 확인하는 핵심적인 메서드.
    // TransitionCondition마다 조건 내용이 다를 것이므로 자식 SO에서 구현
}
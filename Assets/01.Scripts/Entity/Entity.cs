using System;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    #region Components
    public StateMachine StateMachineCompo { get; private set; }
    public AnimatorComponent AnimatroCompo { get; private set; }
    public MoveComponent MoveCompo { get; private set; }
    #endregion

    [Header("Transition Conditions")]
    [SerializeField] private TransitionCondition[] _conditions;
    
    public Dictionary<StateTypeEnum, List<TransitionCondition>> ConditionDictionary { get; private set; }

    protected virtual void Awake() 
    {
        Transform visual = transform.Find("Visual").transform;

        AnimatroCompo = visual.GetComponent<AnimatorComponent>();
        MoveCompo = transform.GetComponent<MoveComponent>();

        SetComponents();    
        SetStates();
        SetTransitionConditions();
    }

    private void Start() 
    {
        StateMachineCompo.Init(StateTypeEnum.Idle);
    }

    private void Update() 
    {
        StateMachineCompo.CurrentState.UpdateState();
    }

    private void SetComponents()
    {
        List<BaseComponent> components = new();
        components.AddRange(transform.GetComponentsInChildren<BaseComponent>());

        foreach (var compo in components)
        {
            compo.SetOwner(this);
        }
    }

    private void SetStates()
    {
        StateMachineCompo = new StateMachine();

        foreach (StateTypeEnum state in Enum.GetValues(typeof(StateTypeEnum)))
        {
            string typeName = state.ToString(); 

            Type t = Type.GetType($"{typeName}State");
            State newState = Activator.CreateInstance(t, this, StateMachineCompo, typeName) as State; 

            if (newState == null)
            {
                Debug.LogError($"There is no script : {state}");
                return;
            }
            StateMachineCompo.AddState(state, newState);
        }
    }

    protected void SetTransitionConditions()
    {
        if (_conditions.Length == 0)
            return;

        ConditionDictionary = new Dictionary<StateTypeEnum, List<TransitionCondition>>();

        foreach (var condition in _conditions)
        {
            var newCondition = condition.OnRegister(this);
            newCondition.Owner = this;
            
            if (!ConditionDictionary.ContainsKey(newCondition.TargetStateType))
                ConditionDictionary[newCondition.TargetStateType] = new List<TransitionCondition>(); 

            ConditionDictionary[newCondition.TargetStateType].Add(newCondition); 
        }
    }

    public void IsConditionsValid(StateTypeEnum targetStateType)
    {
        ConditionDictionary.TryGetValue(targetStateType, out List<TransitionCondition> conditions);

        if (conditions == null) return;

        foreach (var condition in conditions)
        {
            if (condition.IsConditionValid())
            {
                this.StateMachineCompo.ChangeState(targetStateType);
            }
        }

        // 사용 예: IdleState에서 MoveState로 가는 TransitionCondition들을 모두 가져와 확인하고 싶을 때
        //        StateTypeEnum.Move를 매개변수로 넘겨주고 이 함수를 Update에서 실행한다.
        //        조건들 중 하나라도 조건이 만족된다면 MoveState로 이동된다.
    }
}

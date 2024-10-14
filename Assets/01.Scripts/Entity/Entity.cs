using System;
using System.Collections;
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
    [SerializeField] private TransitionCondition[] _conditions; // 상태 전이 조건들
    
    public Dictionary<StateTypeEnum, List<TransitionCondition>> ConditionDictionary { get; private set; }

    protected virtual void Awake() 
    {
        Transform visual = transform.Find("Visual").transform;

        // 모든 Entity에 공통으로 들어가는 필수 컴포넌트들은 모두 여기서 직접 할당
        AnimatroCompo = visual.GetComponent<AnimatorComponent>();
        MoveCompo = transform.GetComponent<MoveComponent>();

        SetComponents();    
        SetStates();
        SetTransitionConditions();
    }

    private void Start() 
    {
        StateMachineCompo.Init(StateTypeEnum.Idle);
        Debug.Log("dma");
    }

    private void Update() 
    {
        StateMachineCompo.CurrentState.UpdateState();
    }

    private void SetComponents()
    {
        List<BaseComponent> components = new();
        components.AddRange(transform.GetComponentsInChildren<BaseComponent>());
        // 컴포넌트들을 모두 긁어와서 리스트에 넣어준다. 본인 오브젝트와 자식 오브젝트까지 모두 돈다.

        foreach (var compo in components)
        {
            compo.SetOwner(this); // 리스트를 순회하며 오너를 세팅해줌
        }
    }

    private void SetStates() // Reflection을 사용하여 런타임에 동적으로 State들에 접근해 추가해주는 로직
    {
        StateMachineCompo = new StateMachine();

        foreach (StateTypeEnum state in Enum.GetValues(typeof(StateTypeEnum))) // StateEnum 모두 돎
        {
            string typeName = state.ToString(); // stateEnum값의 이름 받아옴
            Type t = Type.GetType($"{typeName}State"); // 받아온 이름으로 Type 가져옴 (이름 형식은 "XX State")
            State newState = Activator.CreateInstance(t, this, StateMachineCompo, typeName) as State; 
            // Type을 넣어 리플렉션으로 State 생성. State 생성자의 매개변수들을 넘겨준다. 3번째 매개변수는 State에서 직접 구현하므로 일단 null 넘김

            if (newState == null)
            {
                Debug.LogError($"There is no script : {state}");
                return;
            }
            StateMachineCompo.AddState(state, newState); // StateMachine에 새로 생성한 State 등록
        }
    }

    protected void SetTransitionConditions() // 인스펙터에서 넣어준 TransitionConditon들을 딕셔너리에 넣어주는 함수
    {
        if (_conditions.Length == 0) // TransitionCondition 배열에 아무것도 없다면 리턴
            return;

        ConditionDictionary = new Dictionary<StateTypeEnum, List<TransitionCondition>>();
        // StateTypeEnum -> 상태 전이 조건의 목표 State. 예를 들어 'IsTargetDetected'는 조건이 만족되면 목표 State인 MoveState로 간다. 
        // List<TransitionCondition> -> 목표 State에 맞는 상태 전이 조건들. 하나가 아닐 수 있기 때문에 리스트

        foreach (var condition in _conditions) // TransitionCondition 배열을 반복 돌리고
        {
            var newCondition = condition.OnRegister(this); // Entity끼리의 중복 방지를 위해 Clone을 생성
            newCondition.Owner = this; // 일단 들어온 condition에 오너를 세팅해줌
            
            if (!ConditionDictionary.ContainsKey(newCondition.TargetStateType)) // condition의 TargetStateType이 키값으로 딕셔너리에 존재하지 않을때,
                ConditionDictionary[newCondition.TargetStateType] = new List<TransitionCondition>(); 
                // 딕셔너리에 새로운 리스트를 만들어 밸류로 넣어줌

            ConditionDictionary[newCondition.TargetStateType].Add(newCondition); 
            // 딕셔너리에 존재한다면 condition을 리스트에 추가해줌
        }
    }

    public void IsConditionsValid(StateTypeEnum stateType) // 목표 스테이트 타입을 받아주고
    {
        ConditionDictionary.TryGetValue(stateType, out List<TransitionCondition> conditions);
        // 들어온 스테이트 타입에 맞는 전이 조건 리스트를 딕셔너리에서 빼온다.

        if (conditions == null) return; // 리스트가 없다면 리턴하고

        Debug.Log("Check Condition");
        foreach (var condition in conditions) // 있다면 그 리스트를 반복해
        {
            if (condition.IsConditionValid()) // 조건 리스트 중 만족되는 조건이 있는지 확인한다.
            {
                Debug.Log("Check Condition state");
                this.StateMachineCompo.ChangeState(stateType); // 조건 하나라도 만족된다면 목표 스테이트 타입으로 이동.
            }
        }

        // 사용 예: IdleState에서 MoveState로 가는 TransitionCondition들을 모두 가져와 확인하고 싶을 때
        //        StateTypeEnum.Move를 매개변수로 넘겨주고 이 함수를 Update에서 실행한다.
        //        조건들 중 하나라도 조건이 만족된다면 MoveState로 이동된다.
    }
}

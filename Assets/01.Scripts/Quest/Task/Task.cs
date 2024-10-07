using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TaskState
{
    Inactive,
    Active,
    Complete
}

[CreateAssetMenu(menuName = "SO/Quest/Task", fileName = "Task_")]
public class Task : ScriptableObject
{
    [Header("Text")]
    [SerializeField] private string _description;

    [Header("Target")]
    [SerializeField] private TaskTarget _taskTarget;

    [Header("Setting")]
    [SerializeField] private int _initialSuccessValue = 0;
    [SerializeField] private int _needToSuccessValue;

    private TaskState _taskState;
    private int _currentSuccessValue;

    public int CurrentSuccessValue
    {
        get => _currentSuccessValue;
        set => _currentSuccessValue = value;
    }
    public string Description => _description;
    public int NeedToSuccessValue => _needToSuccessValue;
    public TaskState TaskState => _taskState;

    public bool IsComplete => TaskState == TaskState.Complete;
    public Quest Owner { get; private set; }
     
    public void SetOwner(Quest owner)
    {
        Owner = owner;
    }

    public void Start()
    {
        _taskState = TaskState.Active;
        _currentSuccessValue = _initialSuccessValue;
    }
    
    public void ReceieveReport(object target, int successCount, Quest quest)
    {
        if (!_taskTarget.IsTargetEqual(target)) return;
        if (quest.CodeName != Owner.CodeName) return;
        if (TaskState == TaskState.Complete) return;

        _currentSuccessValue += successCount;
        Debug.Log(_currentSuccessValue);

        if (_currentSuccessValue >= _needToSuccessValue)
        {
            _taskState = TaskState.Complete;
        }
    }
}
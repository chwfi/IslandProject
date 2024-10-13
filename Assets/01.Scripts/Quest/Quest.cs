using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;

[JsonConverter(typeof(StringEnumConverter))]
public enum QuestState
{
    Inactive,
    Active,
    Complete,
    Cancel,
    WaitingForCompletion,
    None
}

public abstract class Quest : ScriptableObject, IQuestable
{
    public delegate void CompletedHandler(Quest quest);
    public delegate void CanceldHandler(Quest quest);
    public delegate void SetUIHandler(Quest quest);
    public delegate void UpdateUIHandler();
    public delegate void DestroyUIHandler(Quest quest);

    [Header("Info")]
    [SerializeField] protected int _codeName;
    [SerializeField] protected string _questName;
    [SerializeField] protected Sprite _questIcon;
    [SerializeField] protected string _questDescription;

    [Header("Reward")]
    [SerializeField] protected RewardGroup[] _rewardGroups;

    [Header("Option")]
    [SerializeField] protected bool _isAutoStartQuest;
    [SerializeField] protected bool _isAutoComplete;
    [SerializeField] protected bool _isCanclable;

    public event CompletedHandler OnCompleted;
    public event CanceldHandler OnCanceled;
    public event SetUIHandler OnSetUI;
    public event UpdateUIHandler OnUpdateUI;
    public event DestroyUIHandler OnDestroyUI;

    public int CodeName
    {
        get => _codeName;
        set => _codeName = value;
    }
    protected QuestState _state;
    public QuestState State
    {
        get => _state;
        set
        {
            _state = value;
            OnUpdateUI?.Invoke();
        }
    }
    public string QuestName => _questName;

    public IReadOnlyList<RewardGroup> Rewards => _rewardGroups;
    public bool IsRegistered => State != QuestState.Inactive;
    public bool IsCompletable => State == QuestState.WaitingForCompletion;
    public bool IsComplete => State == QuestState.Complete;
    public bool IsCancel => State == QuestState.Cancel;
    public virtual bool IsCancelable => _isCanclable;

    public virtual void OnRegister()
    {
        QuestUIController.Instance.SetRegisterUI(this, quest => OnSetUI?.Invoke(this));
    }

    public virtual void OnComplete()
    {
        Debug.Log("Complete Quest!");
        State = QuestState.Complete;

        OnDestroyUI?.Invoke(this);
        OnCompleted?.Invoke(this);

        foreach (var group in _rewardGroups)
            group.reward.Give(this, group.amount);

        OnCompleted = null;
        OnCanceled = null;
    }

    public virtual void Cancel()
    {
        Debug.Assert(IsCancelable, "This quest can't be canceled");

        State = QuestState.Cancel;
        OnCanceled?.Invoke(this);
    }

    public virtual Quest Clone()
    {
        var clone = Instantiate(this);
    
        return clone;
    }
}
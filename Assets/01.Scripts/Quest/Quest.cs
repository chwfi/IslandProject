using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum QuestState
{
    Inactive,
    Active,
    Complete,
    Cancel,
    WaitingForCompletion,
    None
}

[CreateAssetMenu(menuName = "SO/Quest/Quest")]
public class Quest : ScriptableObject
{
    public delegate void CompletedHandler(Quest quest);
    public delegate void CanceldHandler(Quest quest);
    public delegate void SetUIHandler(Quest quest);
    public delegate void UpdateUIHandler(Quest quest);

    [Header("Info")]
    [SerializeField] private int _codeName;
    [SerializeField] private string _questName;
    [SerializeField] private Sprite _questIcon;
    [SerializeField] private string _questDescription;

    [Header("TaskGroup")]
    [SerializeField] private Task[] _taskGroup;

    [Header("Reward")]
    [SerializeField] private RewardGroup[] _rewardGroups;

    [Header("Option")]
    [SerializeField] private bool _isAutoStartQuest;
    [SerializeField] private bool _isAutoComplete;
    [SerializeField] private bool _isCanclable;
    [SerializeField] private bool _isSavable;

    public event CompletedHandler OnCompleted;
    public event CanceldHandler OnCanceled;
    public event SetUIHandler OnSetUI;
    public event UpdateUIHandler OnUpdateUI;

    public Task[] TaskGroup => _taskGroup;
    public int CodeName
    {
        get => _codeName;
        set => _codeName = value;
    }
    private QuestState _state;
    public QuestState State
    {
        get => _state;
        set
        {
            _state = value;
            //UpdateUI();
        }
    }
    public string QuestName => _questName;
    public Sprite QuestIcon => _questIcon;
    public string QuestDescription => _questDescription;

    //public IReadOnlyList<Reward> Rewards => _rewards;
    public bool IsRegistered => State != QuestState.Inactive;
    public bool IsCompletable => State == QuestState.WaitingForCompletion;
    public bool IsComplete => State == QuestState.Complete;
    public bool IsCancel => State == QuestState.Cancel;
    public virtual bool IsCancelable => _isCanclable;
    public virtual bool IsSavable => _isSavable;
    public bool IsAllTaskComplete => _taskGroup.All(x => x.IsComplete);

    //private QuestUI _questUI;

    public void OnRegister()
    {
        Debug.Assert(!IsRegistered, "This quest has already been registered");

        if (_isAutoStartQuest)
            State = QuestState.Active;    

        foreach (var task in _taskGroup)
        {
            task.SetOwner(this);
            task.Start();
        }

        OnRegisterUI();
    }

    public void OnRegisterUI() // 퀘스트에 맞는 UI 생성
    {
        //_questUI = QuestBindingManager.Instance.SetUI(this); // 생성한걸 변수에 할당해줌
        //Debug.Log(_questUI);
        //SetUI(); // 생성 후 바로 세팅해줌
    }

    public void OnReceieveReport(object target, int successCount)
    {
        if (IsComplete)
            return;

        foreach (var task in _taskGroup)
            task.ReceieveReport(target, successCount, this);

        State = QuestState.Active;
    }

    public void OnCheckComplete()
    {
        if (IsAllTaskComplete)
        {
            if (_isAutoComplete)
                Complete();
        }
    }

    public void Complete()
    {
        var questSystem = QuestSystem.Instance;
        State = QuestState.Complete;

        OnCompleted?.Invoke(this);

        foreach (var group in _rewardGroups)
            group.reward.Give(this, group.amount);

        //questSystem.OnQuestRecieved -= OnReceieveReport;
        //questSystem.OnCheckCompleted -= OnCheckComplete;
        //questSystem.OnUpdateQuestUI -= UpdateUI;

        OnCompleted = null;
        OnCanceled = null;
    }

    public virtual void Cancel()
    {
        Debug.Assert(IsCancelable, "This quest can't be canceled");

        State = QuestState.Cancel;
        OnCanceled?.Invoke(this);
    }

    public Quest Clone()
    {
        var clone = Instantiate(this);
        //clone._taskGroup = _taskGroup;

        var questSystem = QuestSystem.Instance;
        
        //questSystem.OnQuestRecieved += clone.OnReceieveReport;
        //questSystem.OnCheckCompleted += clone.OnCheckComplete;
        //questSystem.OnUpdateQuestUI += clone.UpdateUI;

        return clone;
    }

    public QuestSaveData ToSaveData()
    {
        return new QuestSaveData
        {
            codeName = _codeName,
            questState = _state,
            taskSaveData = _taskGroup.Select(task => new TaskSaveData
            {
                currentSuccess = task.CurrentSuccessValue
            }).ToArray()
        };
    }

    public void LoadFrom(QuestSaveData saveData)
    {
        _state = saveData.questState;

        for (int i = 0; i < Mathf.Min(_taskGroup.Length, saveData.taskSaveData.Length); i++)
        {
            _taskGroup[i].CurrentSuccessValue = saveData.taskSaveData[i].currentSuccess;
        }
    }

    #region EventMethods
    // public void UpdateUI()
    // {
    //     if (_questUI == null) return;

    //     OnUpdateUI?.Invoke(this);
    // }

    // public void SetUI()
    // {
    //     if (_questUI == null) return;

    //     OnSetUI?.Invoke(this);
    //     OnUpdateUI?.Invoke(this);
    // }
    #endregion
}
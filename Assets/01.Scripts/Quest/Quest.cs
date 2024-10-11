using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class QuestSaveData
{
    public int codeName;
    public QuestState questState;
    public TaskSaveData[] taskSaveData;
}

public enum QuestState
{
    Inactive,
    Active,
    Complete,
    Cancel,
    WaitingForCompletion,
    None
}

public enum QuestType
{
    TaskQuest,
    TrafficQuest
}

[CreateAssetMenu(menuName = "SO/Quest/Quest")]  
public class Quest : ScriptableObject, ICloneable<Quest>, IQuestable
{
    public delegate void CompletedHandler(Quest quest);
    public delegate void CanceldHandler(Quest quest);
    public delegate void SetUIHandler(Quest quest);
    public delegate void UpdateUIHandler();

    [Header("Info")]
    [SerializeField] private QuestType _questType;
    [SerializeField] private int _codeName;
    [SerializeField] private string _questName;
    [SerializeField] private Sprite _questIcon;
    [SerializeField] private string _questDescription;

    [Header("TaskGroup")]
    [SerializeField] private Task[] _taskGroup;

    [Header("MaterialGroup")]
    [SerializeField] private NeedMaterialGroup[] _materialGroups;

    [Header("Reward")]
    [SerializeField] private RewardGroup[] _rewardGroups;

    [Header("Option")]
    [SerializeField] private bool _isAutoStartQuest;
    [SerializeField] private bool _isAutoComplete;
    [SerializeField] private bool _isCanclable;

    public event CompletedHandler OnCompleted;
    public event CanceldHandler OnCanceled;
    public event SetUIHandler OnSetUI;
    public event UpdateUIHandler OnUpdateUI;

    public QuestType QuestType => _questType;
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
            OnUpdateUI?.Invoke();
        }
    }
    public string QuestName => _questName;

    public IReadOnlyList<RewardGroup> Rewards => _rewardGroups;
    public IReadOnlyList<NeedMaterialGroup> MaterialGroups => _materialGroups;
    public bool IsRegistered => State != QuestState.Inactive;
    public bool IsCompletable => State == QuestState.WaitingForCompletion;
    public bool IsComplete => State == QuestState.Complete;
    public bool IsCancel => State == QuestState.Cancel;
    public virtual bool IsCancelable => _isCanclable;
    public bool IsAllTaskComplete => _taskGroup.All(x => x.IsComplete);
    public bool IsAllMaterialGroupMet => _materialGroups.All(x => x.IsComplete);

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

        foreach (var material in _materialGroups)
        {
            material.SetOwner(this);
            material.Start();
        }

        QuestUIController.Instance.SetRegisterUI(this, quest => OnSetUI?.Invoke(this));
    }

    public void OnReceieveTask(object target, int successCount)
    {
        if (IsComplete)
            return;

        foreach (var task in _taskGroup)
            task.ReceieveReport(target, successCount, this);

        State = QuestState.Active;
    }

    public void OnCheckCompleteTask()
    {
        if (IsAllTaskComplete)
        {
            if (_isAutoComplete)
            {
                OnComplete();
            }
            else
            {
                State = QuestState.WaitingForCompletion;
            }
        }
    }

    public void OnCheckCompleteMaterial()
    {
        foreach (var matGroup in _materialGroups)
        {
            var matData = MaterialManager.Instance.FindMaterialBy(matGroup.material.CodeName);
            Debug.Log(matData);
            if (matData.MaterialCounter.materialCount >= matGroup.needAmount)
            {
                Debug.Log("Material group complete");
                matGroup.Complete();
            }
        }

        if (IsAllMaterialGroupMet)
        {
            Debug.Log("All Complete");
            if (_isAutoComplete)
            {
                OnComplete();
            }
            else
            {
                State = QuestState.WaitingForCompletion;
            }
        }
    }

    public void OnComplete()
    {
        Debug.Log("Complete Quest!");
        State = QuestState.Complete;

        OnCompleted?.Invoke(this);

        foreach (var group in _rewardGroups)
            group.reward.Give(this, group.amount);

        if (QuestType == QuestType.TaskQuest)
        {
            var questSystem = QuestManager.Instance;
            questSystem.OnQuestRecieved -= OnReceieveTask;
            questSystem.OnCheckCompleted -= OnCheckCompleteTask;
        }
        else if (QuestType == QuestType.TrafficQuest)
        {
            var materialManager = MaterialManager.Instance;
            materialManager.OnReceivedNotify -= OnCheckCompleteMaterial;
        }

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
        clone._taskGroup = _taskGroup;
        
        if (QuestType == QuestType.TaskQuest)
        {
            var questSystem = QuestManager.Instance;
            questSystem.OnQuestRecieved += clone.OnReceieveTask;
            questSystem.OnCheckCompleted += clone.OnCheckCompleteTask;
        }
        else if (QuestType == QuestType.TrafficQuest)
        {
            var materialManager = MaterialManager.Instance;
            materialManager.OnReceivedNotify += clone.OnCheckCompleteMaterial;
        }
        
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
        _codeName = saveData.codeName;
        _state = saveData.questState;

        if (_taskGroup.Length > 0)
        {
            for (int i = 0; i < Mathf.Min(_taskGroup.Length, saveData.taskSaveData.Length); i++)
            {
                _taskGroup[i].CurrentSuccessValue = saveData.taskSaveData[i].currentSuccess;
            }     
        }
    }
}
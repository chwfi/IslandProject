using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class TaskQuestSaveData
{
    public int codeName;
    public QuestState questState;
    public TaskSaveData[] taskSaveData;
}

public class TaskSaveData
{
    public int currentSuccess;
}

[CreateAssetMenu(menuName = "SO/Quest/TaskQuest")]  
public class TaskQuest : Quest, ICloneable<Quest>, IQuestable
{
    [Header("TaskGroup")]
    [SerializeField] private Task[] _taskGroup;

    public List<Task> TaskCloneGroup { get; private set; }
    public bool IsAllTaskComplete => TaskCloneGroup.All(x => x.IsComplete);

    public override void OnRegister()
    {
        var questSystem = QuestManager.Instance;
        questSystem.OnQuestRecieved += OnReceieveTask;
        questSystem.OnCheckCompleted += OnCheckCompleteTask;
        
        foreach (var task in _taskGroup)
        {
            var newTask = task.Clone();
            newTask.SetOwner(this);
            newTask.Start();
            TaskCloneGroup.Add(newTask);
        }

        base.OnRegister();
    }

    public void OnReceieveTask(object target, int successCount)
    {
        if (IsComplete)
            return;

        foreach (var task in TaskCloneGroup)
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

    public override void OnComplete()
    {
        base.OnComplete();

        var questSystem = QuestManager.Instance;
        questSystem.OnQuestRecieved -= OnReceieveTask;
        questSystem.OnCheckCompleted -= OnCheckCompleteTask;
    }

    public TaskQuestSaveData ToSaveData()
    {
        return new TaskQuestSaveData
        {
            codeName = _codeName,
            questState = _state,
            taskSaveData = TaskCloneGroup.Select(task => new TaskSaveData
            {
                currentSuccess = task.CurrentSuccessValue
            }).ToArray()     
        };
    }

    public TaskQuestSaveData ToInitialSaveData()
    {
        return new TaskQuestSaveData
        {
            codeName = _codeName,
            questState = _state,
            taskSaveData = _taskGroup.Select(task => new TaskSaveData
            {
                currentSuccess = task.CurrentSuccessValue
            }).ToArray()     
        };
    }

    public void LoadFrom(TaskQuestSaveData saveData)
    {
        _codeName = saveData.codeName;
        _state = saveData.questState;

        if (TaskCloneGroup.Count > 0)
        {
            for (int i = 0; i < Mathf.Min(TaskCloneGroup.Count, saveData.taskSaveData.Length); i++)
            {
                TaskCloneGroup[i].CurrentSuccessValue = saveData.taskSaveData[i].currentSuccess;
            }     
        }
    }
}
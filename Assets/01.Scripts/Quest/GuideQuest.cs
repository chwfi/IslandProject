using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class GuideQuestSaveData
{
    public int codeName;
    public QuestState questState;
    public TaskSaveData taskSaveData;
}

[CreateAssetMenu(menuName = "SO/Quest/GuideQuest")]  
public class GuideQuest : Quest, ICloneable<Quest>, IQuestable
{
    [Header("TaskGroup")]
    [SerializeField] private Task _task;

    [HideInInspector] public Task NewTaskClone;
    public bool IsAllTaskComplete => NewTaskClone.IsComplete;

    public override void OnRegister()
    {
        var questSystem = QuestManager.Instance;
        questSystem.OnQuestRecieved += OnReceieveTask;
        questSystem.OnCheckCompleted += OnCheckCompleteTask;
        
        var newTask = _task.Clone();
        newTask.SetOwner(this);
        newTask.Start();
        NewTaskClone = newTask;
    }

    public void OnReceieveTask(object target, int successCount)
    {
        if (IsComplete)
            return;

        NewTaskClone.ReceieveReport(target, successCount, this);

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

        GuideController.Instance.SetCurrentGuideQuest();

        var questSystem = QuestManager.Instance;
        questSystem.OnQuestRecieved -= OnReceieveTask;
        questSystem.OnCheckCompleted -= OnCheckCompleteTask;
    }

    // public GuideQuestSaveData ToSaveData()
    // {
    //     return new GuideQuestSaveData
    //     {
    //         codeName = _codeName,
    //         questState = _state,
    //         taskSaveData = TaskCloneGroup.First
    //     };
    // }

    // public GuideQuestSaveData ToInitialSaveData()
    // {
    //     return new GuideQuestSaveData
    //     {
    //         codeName = _codeName,
    //         questState = _state,
    //         taskSaveData = _task.CurrentSuccessValue,
    //     };
    // }

    // public void LoadFrom(GuideQuestSaveData saveData)
    // {
    //     _codeName = saveData.codeName;
    //     _state = saveData.questState;

    //     if (TaskCloneGroup.Count > 0)
    //     {
    //         for (int i = 0; i < Mathf.Min(TaskCloneGroup.Count, saveData.taskSaveData.Length); i++)
    //         {
    //             TaskCloneGroup[i].CurrentSuccessValue = saveData.taskSaveData[i].currentSuccess;
    //         }     
    //     }
    // }
}
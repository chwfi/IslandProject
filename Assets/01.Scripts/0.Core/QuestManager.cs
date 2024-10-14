using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class QuestManager : MonoSingleton<QuestManager>
{
    public delegate void QuestRecieveHandler(object target, int successCount);
    public delegate void CheckCompleteHandler();

    [SerializeField] private QuestDatabase _questDatabase;
    [SerializeField] private string _root;

    public List<TaskQuest> ActiveTaskQuests = new List<TaskQuest>();
    public List<TrafficQuest> ActiveTrafficQuests = new List<TrafficQuest>();

    public event QuestRecieveHandler OnQuestRecieved;
    public event CheckCompleteHandler OnCheckCompleted;

    private void Start() 
    {
        Load();  
    }

    private void OnApplicationQuit() 
    {
        Save();    
    }

    public void Report(object target, int successCount)
    {
        OnQuestRecieved?.Invoke(target, successCount);
        OnCheckCompleted?.Invoke();
    }

    #region Save & Load
    public void Load()
    {
        foreach (TaskQuest taskQuest in _questDatabase.TaskQuests)
        {
            OnLoadTaskQuestData(taskQuest);
        }

        foreach (TrafficQuest trafficQuest in  _questDatabase.TrafficQuests)
        {
            OnLoadTrafficQuestData(trafficQuest);
        }
    }

    public void Save()
    {
        Debug.Log("Save");
        DataManager.Instance.OnDeleteData(_root);

        foreach (var taskQuest in ActiveTaskQuests)
        {
            DataManager.Instance.OnSaveData(taskQuest.ToSaveData(), taskQuest.QuestName, _root);
        }

        foreach (var trafficQuest in ActiveTrafficQuests)
        {
            DataManager.Instance.OnSaveData(trafficQuest.ToSaveData(), trafficQuest.QuestName, _root);
        }
    }

    public void SaveTaskQuestAtEmptyState()
    {
        foreach (var taskQuest in _questDatabase.TaskQuests)
        {
            DataManager.Instance.OnSaveData(taskQuest.ToInitialSaveData(), taskQuest.QuestName, _root);
            OnLoadTaskQuestData(taskQuest);
        }
    }

    public void SaveTrafficQuestAtEmptyState()
    {
        foreach (var trafficQuest in _questDatabase.TrafficQuests)
        {
            DataManager.Instance.OnSaveData(trafficQuest.ToSaveData(), trafficQuest.QuestName, _root);
            OnLoadTrafficQuestData(trafficQuest);
        }
    }

    public void OnLoadTaskQuestData(TaskQuest quest)
    {
        DataManager.Instance.OnLoadData<TaskQuestSaveData>(quest.QuestName, _root, (loadedData) =>
        {
            if (loadedData != null)
            {
                if (loadedData.questState == QuestState.Complete)
                    return;

                var newQuest = quest.Clone() as TaskQuest;
                newQuest.OnRegister();
                newQuest.LoadFrom(loadedData);
                ActiveTaskQuests.Add(newQuest);

                OnCheckCompleted?.Invoke();
            }
            else
            {
                Debug.Log("Failed to load data");
            }
        }, () => SaveTaskQuestAtEmptyState());            
    }

    public void OnLoadTrafficQuestData(TrafficQuest quest)
    {
        DataManager.Instance.OnLoadData<TrafficQuestSaveData>(quest.QuestName, _root, (loadedData) =>
        {
            if (loadedData != null)
            {
                if (loadedData.questState == QuestState.Complete)
                    return;

                var newQuest = quest.Clone() as TrafficQuest;
                newQuest.OnRegister();
                newQuest.LoadFrom(loadedData);
                ActiveTrafficQuests.Add(newQuest);
            }
            else
            {
                Debug.Log("Failed to load data");
            }
        }, () => SaveTrafficQuestAtEmptyState());            
    }
    #endregion
}
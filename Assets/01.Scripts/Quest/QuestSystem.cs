using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

[System.Serializable]
public class QuestSaveData
{
    public int codeName;
    public QuestState questState;
    public TaskSaveData[] taskSaveData;
}

public class TaskSaveData
{
    public int currentSuccess;
}

public class QuestSystem : MonoSingleton<QuestSystem>
{
    public delegate void QuestRecieveHandler(object target, int successCount);
    public delegate void CheckCompleteHandler();

    [SerializeField] private QuestDatabase _questDatabase;
    [SerializeField] private string _questRoot;

    public event QuestRecieveHandler OnQuestRecieved;
    public event CheckCompleteHandler OnCheckCompleted;

    private void Start() 
    {
        Load();  
    }

    private void OnApplicationQuit() 
    {
        OnSaveQuestData();    
    }

    public void Report(object target, int successCount)
    {
        OnQuestRecieved?.Invoke(target, successCount);
        OnCheckCompleted?.Invoke();
    }

    #region Save & Load
    public void Load()
    {
        foreach (Quest quest in _questDatabase.Quests)
        {
            OnLoadQuestData(quest);
        }
    }

    public void OnSaveQuestData()
    {
        DataManager.Instance.OnDeleteData(_questRoot);

        foreach (var quest in _questDatabase.Quests)
        {
            DataManager.Instance.OnSaveData(quest.ToSaveData(), quest.QuestName, _questRoot);
        }
    }

    public void OnLoadQuestData(Quest quest)
    {
        DataManager.Instance.OnLoadData<QuestSaveData>(quest.QuestName, _questRoot, (loadedData) =>
        {
            if (loadedData != null)
            {
                var newQuest = quest.Clone();
                newQuest.OnRegister();  
                quest.LoadFrom(loadedData);
                Debug.Log("Success to load data");
            }
            else
            {
                Debug.Log("Failed to load data");
            }
        });            
    }
    #endregion
}
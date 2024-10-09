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
    [SerializeField] private QuestDatabase _questDatabase;

    private void Awake() 
    {
        Load();    
    }

    private void Start() 
    {
        
    }

    public void Load()
    {
        foreach (Quest quest in _questDatabase.Quests)
        {
            LoadActiveQuest(quest);
        }
    }

    private void LoadActiveQuest(Quest quest)
    {
        var newQuest = quest.Clone();
        newQuest.OnRegister();  
        OnLoadQuestData(newQuest);
    }

    public void OnSaveQuestData()
    {
        foreach (var quest in _questDatabase.Quests)
        {
            DataManager.Instance.OnSaveData(quest.ToSaveData(), quest.QuestName);
        }
    }

    public void OnLoadQuestData(Quest quest)
    {
        DataManager.Instance.OnLoadData<QuestSaveData>(quest.QuestName, (loadedData) =>
        {
            if (loadedData != null)
            {
                //quest.OnRegister();
                quest.LoadFrom(loadedData);
                Debug.Log("Success to load data");
                //quest.OnRegister();
            }
            else
            {
                Debug.Log("Failed to load data");
            }
        });            
    }
}

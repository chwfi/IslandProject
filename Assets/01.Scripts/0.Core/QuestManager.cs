using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class TaskSaveData
{
    public int currentSuccess;
}

public class QuestManager : MonoSingleton<QuestManager>
{
    public delegate void QuestRecieveHandler(object target, int successCount);
    public delegate void CheckCompleteHandler();

    [SerializeField] private QuestDatabase _questDatabase;
    [SerializeField] private string _root;

    public List<Quest> Quests = new List<Quest>();

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
        foreach (Quest quest in _questDatabase.Quests)
        {
            OnLoadQuestData(quest);
        }
    }

    public void Save()
    {
        DataManager.Instance.OnDeleteData(_root);

        foreach (var quest in Quests)
        {
            DataManager.Instance.OnSaveData(quest.ToSaveData(), quest.QuestName, _root);
        }
    }

    public void OnLoadQuestData(Quest quest)
    {
        DataManager.Instance.OnLoadData<QuestSaveData>(quest.QuestName, _root, (loadedData) =>
        {
            if (loadedData != null)
            {
                var newQuest = quest.Clone();
                newQuest.OnRegister();
                newQuest.LoadFrom(loadedData);
                Quests.Add(newQuest);
                Debug.Log("Success to load data");
            }
            else
            {
                Debug.Log("Failed to load data");
            }
        }, () => Save());            
    }
    #endregion
}
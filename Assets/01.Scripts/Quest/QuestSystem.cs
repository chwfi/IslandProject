using System.Collections;
using System.Collections.Generic;
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

    public void OnSaveQuestData()
    {
        foreach (var quest in _questDatabase.Quests)
        {
            DataManager.Instance.OnSaveData(quest.ToSaveData(), quest.QuestName);
        }
    }

    public void OnLoadQuestData()
    {
        foreach (var quest in _questDatabase.Quests)
        {
            DataManager.Instance.OnLoadData<QuestSaveData>(quest.QuestName, (loadedData) =>
            {
                if (loadedData != null)
                {
                    quest.LoadFrom(loadedData);
                }
                else
                {
                    Debug.Log("Failed to load data");
                }
            });
        }
    }
}

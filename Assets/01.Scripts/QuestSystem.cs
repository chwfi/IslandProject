using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct QuestSaveData
{
    public string questName;
    public int questNumber;
}

public class QuestSystem : MonoBehaviour
{
    public QuestSaveData saveData;

    public void OnSaveQuestData()
    {
        DataManager.Instance.OnSaveData(saveData);
    }
}

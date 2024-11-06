using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectStateManager : MonoSingleton<ObjectStateManager>
{
    [SerializeField] private string _saveRoot;

    public List<PlaceableObject> ObjectList { get; set; }

    private void Awake() 
    {
        ObjectList = new List<PlaceableObject>();
    }

    private void Start() 
    {
        foreach (var obj in ObjectList)
        {
            OnLoadData(obj);
        }    
    }

    private void Update() 
    {
        SetObjectTimer();
    }

    private void SetObjectTimer()
    {
        if (ObjectList.Count <= 0)
            return;

        foreach (var obj in ObjectList)
        {
            obj.SetTimer(obj.ObjectData.timer);
        }
    }

    private void OnApplicationQuit() 
    {
        Save();
    }

    public void Save()
    {
        DataManager.Instance.OnDeleteData(_saveRoot);

        if (ObjectList.Count <= 0)
            return;

        int i = 0;

        foreach (var obj in ObjectList)
        {
            DataManager.Instance.OnSaveData(obj.ToSaveData(), 
            $"{obj.ObjectData.objectName}{i++}", _saveRoot);
        }
    }

    public void OnLoadData(PlaceableObject obj)
    {
        DataManager.Instance.OnLoadData<TaskQuestSaveData>(obj.ObjectData.objectName, _saveRoot, (loadedData) =>
        {
            if (loadedData != null)
            {
                if (loadedData.questState == QuestState.Complete)
                    return;

                var newObject = PoolManager.Instance.Take(obj.ObjectData.objectName, null);
                // newObject.
                // newQuest.OnRegister();
                // newQuest.LoadFrom(loadedData);
                // ActiveTaskQuests.Add(newQuest);

                // OnCheckCompleted?.Invoke();
            }
            else
            {
                Debug.Log("Failed to load data");
            }
        }, null);            
    }
}

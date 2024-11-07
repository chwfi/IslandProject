using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectStateManager : MonoSingleton<ObjectStateManager>
{
    [SerializeField] private string _saveRoot;
    [SerializeField] private PlaceableDatabase _database;

    public List<PlaceableObject> ObjectList = new List<PlaceableObject>();

    private void Start() 
    {
        Load();
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

    public void Load()
    {
        DataManager.Instance.OnLoadAllData<PlaceableObjectSaveData>(_saveRoot, (loadedDatas) =>
        {
            if (loadedDatas != null)
            {
                foreach (var data in loadedDatas)
                {
                    var newObject = PoolManager.Instance.Take(data.thisObjectName, null) as PlaceableObject;
                    newObject.LoadFrom(data);
                    ObjectList.Add(newObject);
                }
            }
            else
            {
                Debug.Log("Failed to load data");
            }
        });
    }
}

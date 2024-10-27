using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldStateManager : MonoSingleton<FieldStateManager>
{
    [SerializeField] private string _saveRoot;
    [SerializeField] private float _growthEndTime;

    public List<ObjectField> FieldList { get; set; }

    private void Awake() 
    {
        FieldList = new List<ObjectField>();    
    }

    private void Start() 
    {
        Load();    
    }

    private void Update() 
    {
        if (FieldList.Count <= 0)
            return;

        foreach (var field in FieldList)
        {
            field.SetTimer(_growthEndTime);
        }
    }

    private void OnApplicationQuit() 
    {
        Save();
    }

    public void Save()
    {
        DataManager.Instance.OnDeleteData(_saveRoot);

        if (FieldList.Count <= 0)
            return;

        int i = 0;

        foreach (var field in FieldList)
        {
            DataManager.Instance.OnSaveData(field.ToSaveData(), 
            $"{field.ObjectData.objectName}{i++}", _saveRoot);
        }
    }

    public void Load()
    {
        DataManager.Instance.OnLoadAllData<FieldSaveData>(_saveRoot, (loadedDataList) =>
        {
            if (loadedDataList == null || loadedDataList.Count <= 0)
            {
                Debug.Log("No saved data found");
                return;
            }

            // 불러온 모든 데이터에 대해 ObjectField 오브젝트를 생성 및 초기화
            foreach (var loadedData in loadedDataList)
            {
                var fieldPrefab = PoolManager.Instance.Take("CornField", null) as ObjectField;
                fieldPrefab.transform.position = loadedData.placedPosition;
                if (fieldPrefab != null)
                {
                    fieldPrefab.LoadFrom(loadedData);
                    Debug.Log(fieldPrefab);
                }
                else
                {
                    Debug.LogWarning("Failed to instantiate or find field object for loading");
                }
            }
        });       
    }
}

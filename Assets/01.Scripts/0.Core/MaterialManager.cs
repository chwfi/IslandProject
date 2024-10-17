using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MaterialManager : MonoSingleton<MaterialManager> // Manage In-Game Materials
{
    public delegate void MaterialRecieveHandler(InGameMaterial target, int amount);
    public delegate void NotifyHandler();

    [SerializeField] private MaterialDatabase _materialDatabase;
    [SerializeField] private string _root;

    public List<InGameMaterial> ActiveMaterials = new List<InGameMaterial>();

    public event MaterialRecieveHandler OnMaterialRecieved;
    public event NotifyHandler OnReceivedNotify;

    private void Start() 
    {
        Load();
    }

    private void OnApplicationQuit() 
    {
        Save();
    }

    public void AddMaterialCount(InGameMaterial target, int amount)
    {
        OnMaterialRecieved?.Invoke(target, amount);
        OnReceivedNotify?.Invoke();
    }

    public InGameMaterial FindMaterialBy(int codeName)
    {
        return ActiveMaterials.FirstOrDefault(x => x.CodeName == codeName);
    }
    
    #region Save & Load
    public void Load()
    {
        foreach (var mat in _materialDatabase.Materials) 
        {
            OnLoadMaterialData(mat);
        }
    }

    public void Save() 
    {
        DataManager.Instance.OnDeleteData(_root);

        foreach (var mat in ActiveMaterials)
        {
            DataManager.Instance.OnSaveData(mat.ToSaveData(), mat.MaterialName, _root);
        }
    }

    public void SaveAtEmptyState() // Firebase DB에 데이터가 감지되지 않을때, 즉 처음 실행할 때만 실행해주는 Save 로직
    {
        foreach (var mat in _materialDatabase.Materials)
        {
            DataManager.Instance.OnSaveData(mat.ToInitialSaveData(), mat.MaterialName, _root);
            OnLoadMaterialData(mat);
        }
    }

    public void OnLoadMaterialData(InGameMaterial mat)
    {
        DataManager.Instance.OnLoadData<MaterialSaveData>(mat.MaterialName, _root, (loadedData) =>
        {
            if (loadedData != null)
            {
                var newMat = mat.Clone();
                newMat.LoadFrom(loadedData);
                ActiveMaterials.Add(newMat);

                OnReceivedNotify?.Invoke();
            }
            else
            {
                Debug.Log("Failed to load data");
            }
        }, () => SaveAtEmptyState());            
    }
    #endregion
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MaterialManager : MonoSingleton<MaterialManager> // Manage In-Game Materials
{
    public delegate void MaterialRecieveHandler(InGameMaterial target, int amount);
    public delegate void NotifyHandler();

    [SerializeField] private MaterialDatabase _materialDatabase;
    [SerializeField] private string _root;

    public List<InGameMaterial> Materials = new List<InGameMaterial>();

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
        return Materials.FirstOrDefault(x => x.CodeName == codeName);
    }
    
    #region Save & Load
    public void Load()
    {
        foreach (var mat in _materialDatabase.MaterialList)
        {
            OnLoadMaterialData(mat);
        }
    }

    public void Save()
    {
        DataManager.Instance.OnDeleteData(_root);

        foreach (var mat in Materials)
        {
            DataManager.Instance.OnSaveData(mat.ToSaveData(), mat.MaterialName, _root);
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
                Materials.Add(newMat);
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

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MaterialManager : MonoSingleton<MaterialManager> // Manage In-Game Materials
{
    [SerializeField] private MaterialDatabase _materialDatabase;

    public List<InGameMaterial> MaterialList;

    public Dictionary<string, MaterialCounter> MaterialDictionary { get; private set; }

    private void Awake() 
    {
        MaterialDictionary = new Dictionary<string, MaterialCounter>();

        foreach (InGameMaterial mat in _materialDatabase.MaterialList)
        {
            var newMat = mat.Clone();
            MaterialList.Add(newMat);
            MaterialDictionary.Add(newMat.MaterialName, new MaterialCounter());
        }
    }

    public void AddMaterialCount(string key, int amount)
    {
        MaterialDictionary.TryGetValue(key, out MaterialCounter counter);
        counter.materialCount += amount;
    }

    public MaterialCounter GetMaterialCounter(string key)
    {
        MaterialDictionary.TryGetValue(key, out MaterialCounter counter);
        return counter;
    }

    public InGameMaterial FindQuestBy(Sprite icon) => MaterialList.FirstOrDefault(x => x.Icon.name == icon.name);
}

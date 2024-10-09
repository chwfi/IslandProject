using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MaterialManager : MonoSingleton<MaterialManager> // Manage In-Game Materials
{
    [SerializeField] private MaterialDatabase _materialDatabase;

    [HideInInspector] public List<InGameMaterial> MaterialList;

    private void Awake() 
    {
        foreach (InGameMaterial mat in _materialDatabase.MaterialList)
        {
            MaterialList.Add(mat.Clone());
        }
    }

    public InGameMaterial FindQuestBy(Sprite icon) => MaterialList.FirstOrDefault(x => x.Icon.name == icon.name);
}

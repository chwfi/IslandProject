using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialRepositoryUI : PopupUI
{
    [SerializeField] private Transform _prefabTransform;

    public Dictionary<int, MaterialUI> MaterialUIDictionary = new();

    private void Start() 
    {
        foreach (var mat in MaterialManager.Instance.ActiveMaterials)
        {
            var ui = PoolManager.Instance.Take("MaterialUI", _prefabTransform) as MaterialUI;
            ui.SetUI(mat);
            MaterialUIDictionary.Add(mat.CodeName, ui);
        }
    }

    public override void AccessUI(bool active)
    {
        base.AccessUI(active);

        CreateUI();
    }

    private void CreateUI()
    {
        if (MaterialManager.Instance.ActiveMaterials.Count <= 0)
            return;

        foreach (var mat in MaterialManager.Instance.ActiveMaterials)
        {
            if (MaterialUIDictionary.ContainsKey(mat.CodeName))
            {
                MaterialUIDictionary.TryGetValue(mat.CodeName, out MaterialUI UI);
                UI.UpdateUI(mat.MaterialCounter.materialCount);
            }  
            else
            {
                var ui = PoolManager.Instance.Take("MaterialUI", _prefabTransform) as MaterialUI;
                ui.SetUI(mat);
                MaterialUIDictionary.Add(mat.CodeName, ui);
            }
        }
    }
}

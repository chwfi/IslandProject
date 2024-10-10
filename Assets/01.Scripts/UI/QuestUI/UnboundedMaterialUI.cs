using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnboundedMaterialUI : UnboundedUI
{
    public NeedMaterialGroup OwnMaterial;

    public override void UpdateUI()
    {
        _icon.sprite = OwnMaterial.material.Icon;
        var counter = MaterialManager.Instance.GetMaterialCounter(OwnMaterial.material.MaterialName);
        Debug.Log(counter);
        _amountText.text = $"{counter.materialCount}/{OwnMaterial.needAmount}";
    }
}

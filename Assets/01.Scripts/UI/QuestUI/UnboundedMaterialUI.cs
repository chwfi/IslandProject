using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnboundedMaterialUI : UnboundedUI
{
    private NeedMaterialGroup _group;
    private InGameMaterial _material;

    public void SetUp(NeedMaterialGroup group)
    {
        _group = group;
        _material = MaterialManager.Instance.FindMaterialBy(group.material.CodeName);
    }

    public override void UpdateUI()
    {
        _icon.sprite = _material.Icon;
        _amountText.text = $"{_material.MaterialCounter.materialCount}/{_group.needAmount}";
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnboundedMaterialUI : UnboundedUI
{
    public NeedMaterialGroup OwnMaterial;

    public override void UpdateUI()
    {
        _icon.sprite = OwnMaterial.material.Icon;
        _amountText.text = $"{OwnMaterial.material.currentCount}/{OwnMaterial.needAmount}";
    }
}

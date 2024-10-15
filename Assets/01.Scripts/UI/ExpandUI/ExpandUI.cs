using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ExpandUI : PopupUI
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI _goldAmountText;

    private Zone _zone;

    public void SetUI(Zone zoneData)
    {
        _zone = zoneData;
        _goldAmountText.text = $"{_zone.ExpandPrice}";

        AccessUI(true);
    }

    public override void AccessUI(bool active)
    {
        if (!active)
        {
            _zone.Camera.SetActive(false);
            _zone.ResetMaterial();
        }

        base.AccessUI(active);
    }
}

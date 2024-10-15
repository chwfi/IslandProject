using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ExpandUI : PopupUI
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI _goldAmountText;

    private Zone _currentZone;
    private ExpandButtonUI _expandButton;

    public override void Awake()
    {
        base.Awake();
        
        _expandButton = transform.GetComponentInChildren<ExpandButtonUI>();
    }

    public void SetUI(Zone zoneData)
    {
        _currentZone = zoneData;
        _goldAmountText.text = $"{_currentZone.ExpandPrice}";

        AccessUI(true);
        SetButton(zoneData);
    }

    public void SetButton(Zone zone)
    {
        _expandButton.SetButton(zone);
    }

    public override void AccessUI(bool active)
    {
        if (!active)
        {
            _currentZone.DisableZoneElements();
        }

        base.AccessUI(active);
    }
}

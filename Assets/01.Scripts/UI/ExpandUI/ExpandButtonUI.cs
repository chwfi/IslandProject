using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandButtonUI : ButtonUI
{
    private Zone _previousZone;
    private Zone _currentZone;

    public void SetButton(Zone zone)
    {
        _previousZone = _currentZone;
        _currentZone = zone;

        if (_previousZone != null)
            _button.onClick.RemoveAllListeners();

        _button.onClick.AddListener(() => ZoneManager.Instance.DisableZone(_currentZone));
    }
}

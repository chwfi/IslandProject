using System.Collections.Generic;
using UnityEngine;

public class ZoneManager : MonoSingleton<ZoneManager>
{
    [SerializeField] private Color _selectedColor;

    private ExpandUI _panel;

    public Zone PreviousZone;
    public Zone CurrentZone;

    private void Awake() 
    {
        _panel = PopupUIManager.Instance.transform.GetComponentInChildren<ExpandUI>();
    }

    public void SetZone(Zone zone)
    {
        PreviousZone = CurrentZone;
        CurrentZone = zone;
        
        if (PreviousZone != null)
            PreviousZone.DisableZoneElements();

        CurrentZone.SetZoneElements(_selectedColor);

        _panel.SetUI(CurrentZone);
    }

    public void DisableZone(Zone zone)
    {
        zone.DestroyZone();
        _panel.AccessUI(false);
    }
}

using UnityEngine;

public class ZoneManager : MonoSingleton<ZoneManager>
{
    [Header("Outline Setting")]
    [SerializeField] private Color _selectedColor;

    public Zone PreviousZone { get; private set; }
    public Zone CurrentZone { get; private set; }

    private ExpandUI _panel;

    private void Start() 
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

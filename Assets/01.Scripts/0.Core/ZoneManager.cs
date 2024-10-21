using UnityEngine;

public class ZoneManager : MonoSingleton<ZoneManager>
{
    [Header("Outline Setting")]
    [SerializeField] private Color _outlineColor;
    [SerializeField] private float _outlineWidth;

    public Color OutlineColor => _outlineColor;
    public float OutlineWidth => _outlineWidth;

    public Zone PreviousZone { get; private set; }
    public Zone CurrentZone { get; private set; }

    private ExpandUI _panel;

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

        CurrentZone.SetZoneElements();
        _panel.SetUI(CurrentZone);
    }

    public void DisableZone(Zone zone)
    {
        zone.DestroyZone();
        _panel.AccessUI(false);
    }
}

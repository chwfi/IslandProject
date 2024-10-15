using System.Collections.Generic;
using UnityEngine;

public class ZoneManager : MonoSingleton<ZoneManager>
{
    [SerializeField] private Color _selecteColor;

    private ExpandUI _panel;

    public List<Zone> ClickedZoneList = new List<Zone>();

    private void Awake() 
    {
        _panel = PopupUIManager.Instance.transform.GetComponentInChildren<ExpandUI>();
    }

    public void SetZone(Zone zone)
    {
        if (ClickedZoneList.Count > 0)
        {
            var prevZone = ClickedZoneList[0];
            prevZone.Camera.SetActive(false);
            prevZone.ResetMaterial();
            ClickedZoneList.Remove(prevZone);
        }

        ClickedZoneList.Add(zone);

        zone.Camera.SetActive(true);
        _panel.SetUI(zone);
        zone.SetMaterialProperty(_selecteColor);
    }
}

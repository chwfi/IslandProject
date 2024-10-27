using System.Collections.Generic;
using UnityEngine;

public class PopupUIManager : MonoSingleton<PopupUIManager>, IPoolable
{
    public Dictionary<string, PopupUI> PopupDictionary = new();

    private void Start()
    {
        var list = GetComponentsInChildren<PopupUI>();

        foreach (var popup in list)
        {
            if (!PopupDictionary.ContainsKey(popup.name))
                PopupDictionary.Add(popup.name, popup);
        }
    }

    public void AccessPopupUI(string popupName, bool value)
    {
        PopupDictionary[popupName].AccessUI(value);
    }

    public void MovePopupUI(string popupName, Vector3 dir)
    {
        PopupDictionary[popupName].MoveUI(dir);
    }

    public void OnTakenFromPool()
    {

    }

    public void OnReturnedToPool()
    {

    }
}
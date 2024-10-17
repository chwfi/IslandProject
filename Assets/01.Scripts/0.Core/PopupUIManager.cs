using System.Collections.Generic;

public class PopupUIManager : MonoSingleton<PopupUIManager>
{
    public Dictionary<string, PopupUI> PopupDictionary = new();

    private void Awake()
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
}
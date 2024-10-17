using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildOptionUI : WorldUI
{
    [SerializeField] private Button _placeButton;
    [SerializeField] private Button _exitButton;

    public void SetObject(PlaceableObject obj)
    {
        _placeButton.onClick.AddListener(() => obj.OnPlace());
        _exitButton.onClick.AddListener(() => PlaceManager.Instance.CancelPlace());
    }

    private void OnDestroy() 
    {
        _placeButton.onClick.RemoveAllListeners();
        _exitButton.onClick.RemoveAllListeners();    
    }
}

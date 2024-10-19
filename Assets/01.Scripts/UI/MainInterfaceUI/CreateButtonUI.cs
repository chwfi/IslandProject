using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateButtonUI : ButtonUI
{
    private void Start() 
    {
        SetSubscriptionSelf(() => PopupUIManager.Instance.MovePopupUI("CreatePanel", new Vector3(0, -14, 0)));   
    }
}
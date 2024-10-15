using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HomeBuilding : BaseBuilding
{
    private void OnMouseDown()
    {   
        if (IsClickable())
        {
            PopupUIManager.Instance.SetPopupUI("QuestPanel", true);
        }
    }
}

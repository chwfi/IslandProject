using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeBuilding : BaseBuilding
{
    protected override void OnMouseDown()
    {
        base.OnMouseDown();

        PopupUIManager.Instance.SetPopupUI("QuestPanel", true);
    }
}

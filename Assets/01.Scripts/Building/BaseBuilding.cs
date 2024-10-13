using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BaseBuilding : MonoBehaviour
{
    private void OnMouseDown() 
    {
        PopupUIManager.Instance.SetPopupUI("QuestPanel", true);
    }
}

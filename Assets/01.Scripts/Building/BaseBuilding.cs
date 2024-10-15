using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BaseBuilding : MonoBehaviour
{
    protected virtual void OnMouseDown() 
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
    }
}

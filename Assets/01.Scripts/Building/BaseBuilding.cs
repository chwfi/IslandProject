using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BaseBuilding : MonoBehaviour
{
    protected bool IsClickable()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
            return true;
        else
            return false;
    }
}

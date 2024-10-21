using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Place/PlaceableObject", fileName = "Object_")]
public class PlaceableObjectData : ScriptableObject
{
    public Sprite icon;
    public string objectName;
    public string uiName;
    public int price;
}

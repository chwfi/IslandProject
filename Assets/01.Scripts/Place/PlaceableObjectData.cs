using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlaceableObjectType
{
    LandType,
    WaterType,
}

[CreateAssetMenu(menuName = "SO/Place/PlaceableObject", fileName = "Object_")]
public class PlaceableObjectData : ScriptableObject
{
    [Header("Info")]
    public Sprite icon;
    public string objectName;
    public string uiName;
    public int price;
    public float timer;
    public PlaceableObjectType placeableObjectType;
}

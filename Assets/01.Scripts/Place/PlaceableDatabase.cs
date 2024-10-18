using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Place/PlaceableDatabase")]
public class PlaceableDatabase : ScriptableObject
{
    [SerializeField] private List<PlaceableObjectData> _placeableObjects;
    public List<PlaceableObjectData> PlaceableObjects => _placeableObjects;
}

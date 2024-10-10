using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Material/InGameMaterial", fileName = "Material_")]
public class InGameMaterial : ScriptableObject, ICloneable<InGameMaterial>
{
    [SerializeField] private Sprite _icon;
    [SerializeField] private string _materialName;
    [SerializeField] private string _description;

    public Sprite Icon => _icon;
    public string MaterialName => _materialName;
    public string Description => _description;

    public InGameMaterial Clone()
    {
        var clone = Instantiate(this);
        return clone;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InGameMaterial : ScriptableObject, ICloneable<InGameMaterial>
{
    [SerializeField] private Sprite _icon;
    [SerializeField] private string _description;

    public Sprite Icon => _icon;
    public string Description => _description;

    public int currentCount;

    public InGameMaterial Clone()
    {
        var clone = Instantiate(this);
        return clone;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Reward : ScriptableObject
{
    [SerializeField] private Sprite _icon;
    [SerializeField] private string _description;

    public Sprite Icon => _icon;
    public string Description => _description;

    public abstract void Give(Quest quest, int amount);
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Quest/Reward", fileName = "Reward_")]
public class Reward : ScriptableObject, IQuestable
{
    [SerializeField] private Sprite _icon;  
    [SerializeField] private string _description;

    public Sprite Icon => _icon;
    public string Description => _description;

    public void Give(Quest quest, int amount)
    {
        // 자원 매니저 기능 넣기
    }
}
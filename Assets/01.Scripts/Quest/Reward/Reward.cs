using UnityEngine;

public enum RewardTypeEnum
{
    Coin,
    Popularity,
    Crystal,
}

[CreateAssetMenu(menuName = "SO/Quest/Reward", fileName = "Reward_")]
public class Reward : ScriptableObject, IQuestable
{
    [SerializeField] private Sprite _icon;  
    [SerializeField] private RewardTypeEnum _rewardType;
    [SerializeField] private string _description;

    public Sprite Icon => _icon;
    public RewardTypeEnum RewardType => _rewardType;
    public string Description => _description;

    public void Give(RewardTypeEnum rewardType, int amount)
    {
        ItemManager.Instance.RecieveItem(rewardType, amount);
    }
}
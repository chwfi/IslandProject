using UnityEngine;

[System.Serializable]
public class LevelData
{
    public int level;
    public int requiredExp;
    public int currentExp;
}

public class LevelManager : MonoSingleton<LevelManager>
{
    private const int BASE_REQUIRED_EXP = 250;
    private const float EXP_INCREASE_RATE = 1.2f;

    private int _currentLevel = 1;
    private int _currentExp = 0;
    private int _expForCurrentLevel = 0; // 현재 레벨에서의 경험치

    public int CurrentLevel => _currentLevel;
    public int CurrentExp => _currentExp;
    public int RequiredExp => CalculateRequiredExp(_currentLevel);
    
    private void Start()
    {
        ItemManager.Instance.OnPopularityUpdateUI += UpdateExp;
        UpdateExp(ItemManager.Instance.Popularity);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ItemManager.Instance.RecieveItem(RewardTypeEnum.Popularity, 15);
        }
    }

    private void UpdateExp(int popularity)
    {
        _currentExp = popularity;
        CheckLevelUp();
    }

    private void CheckLevelUp()
    {
        // 총 누적 경험치로 레벨 계산
        int tempExp = _currentExp;
        int level = 1;
        
        while (tempExp >= CalculateRequiredExp(level))
        {
            tempExp -= CalculateRequiredExp(level);
            level++;

            PopupUIManager.Instance.AccessPopupUI("LevelUpPanel", true);
        }

        _currentLevel = level;
        _expForCurrentLevel = tempExp; // 현재 레벨에서의 순수 경험치

        LevelUI.Instance.UpdateLevelUI
        (
            _currentLevel, 
            _expForCurrentLevel,  // 현재 레벨에서의 순수 경험치
            CalculateRequiredExp(_currentLevel)
        );
    }

    private int CalculateRequiredExp(int level)
    {
        return Mathf.RoundToInt(BASE_REQUIRED_EXP * Mathf.Pow(EXP_INCREASE_RATE, level - 1));
    }

    public float GetExpPercentage()
    {
        return (float)_expForCurrentLevel / RequiredExp;
    }
}
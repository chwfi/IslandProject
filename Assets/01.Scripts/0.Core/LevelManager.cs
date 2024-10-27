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
    private const float EXP_INCREASE_RATE = 1.2f; // 레벨당 20% 증가

    private int _currentLevel = 1;
    private int _currentExp = 0;

    public int CurrentLevel => _currentLevel;
    public int CurrentExp => _currentExp;
    public int RequiredExp => CalculateRequiredExp(_currentLevel);

    [SerializeField] private LevelUI _levelUI;

    private void Start()
    {
        ItemManager.Instance.OnPopularityUpdateUI += UpdateExp;
        UpdateExp(ItemManager.Instance.Popularity);
    }

    private void UpdateExp(int popularity)
    {
        _currentExp = popularity;
        CheckLevelUp();
    }

    private void CheckLevelUp()
    {
        while (_currentExp >= CalculateRequiredExp(_currentLevel))
        {
            _currentLevel++;
            // 레벨업 이벤트 처리 (필요시)
            OnLevelUp();
        }
        
        _levelUI.UpdateLevelUI(_currentLevel, _currentExp, CalculateRequiredExp(_currentLevel));
    }

    private int CalculateRequiredExp(int level)
    {
        // 레벨이 올라갈수록 필요 경험치가 증가
        return Mathf.RoundToInt(BASE_REQUIRED_EXP * Mathf.Pow(EXP_INCREASE_RATE, level - 1));
    }

    private void OnLevelUp()
    {
        // 레벨업 시 보상이나 효과 처리
        Debug.Log($"Level Up! Current Level: {_currentLevel}");
    }

    public float GetExpPercentage()
    {
        return (float)_currentExp / RequiredExp;
    }
}
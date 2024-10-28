using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUI : MonoSingleton<LevelUI>
{
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private Image _expSlider;
    [SerializeField] private TextMeshProUGUI _expText;

    public void UpdateLevelUI(int level, int currentLevelExp, int requiredExp)
    {
        _levelText.text = $"{level}";
        _expSlider.fillAmount = (float)currentLevelExp / requiredExp;
        _expText.text = $"{currentLevelExp:N0} / {requiredExp:N0}";
    }
}

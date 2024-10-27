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

    public void UpdateLevelUI(int level, int currentExp, int requiredExp)
    {
        _levelText.text = $"Lv.{level}";
        _expSlider.fillAmount = (float)currentExp / requiredExp;
        _expText.text = $"{currentExp:N0} / {requiredExp:N0}";
    }
}

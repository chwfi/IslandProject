using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MaterialUI : PoolableMono
{
    private Image _icon;
    private TextMeshProUGUI _amountText;

    private void Awake() 
    {
        _icon = transform.Find("Icon").GetComponent<Image>();
        _amountText = transform.Find("AmountText").GetComponent<TextMeshProUGUI>();
    }

    public void SetUI(InGameMaterial material)
    {
        _icon.sprite = material.Icon;
        _amountText.text = $"{material.MaterialCounter.materialCount}";
    }

    public void UpdateUI(int amount)
    {
        _amountText.text = $"{amount}";
    }
}

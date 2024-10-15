using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinUI : MonoBehaviour
{
    private TextMeshProUGUI _amountText;

    private void Awake() 
    {
        _amountText = transform.GetComponentInChildren<TextMeshProUGUI>();

        ItemManager.Instance.OnCoinUpdateUI += UpdateUI;
    }

    private void UpdateUI(int amount)
    {
        _amountText.text = $"{amount}";
    }
}

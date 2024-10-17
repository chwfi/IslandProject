using TMPro;
using UnityEngine;

public class CrystalUI : MonoBehaviour
{
    private TextMeshProUGUI _amountText;

    private void Awake()
    {
        _amountText = transform.GetComponentInChildren<TextMeshProUGUI>();

        ItemManager.Instance.OnCrystalUpdateUI += UpdateUI;
    }

    private void UpdateUI(int amount)
    {
        _amountText.text = $"{amount}";
    }
}

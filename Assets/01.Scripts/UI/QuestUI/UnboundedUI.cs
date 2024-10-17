using TMPro;
using UnityEngine.UI;

public abstract class UnboundedUI : PoolableMono
{
    protected TextMeshProUGUI _amountText;
    protected Image _icon;

    private void OnEnable() 
    {
        _amountText = GetComponentInChildren<TextMeshProUGUI>();
        _icon = GetComponentInChildren<Image>();
    }  

    public abstract void UpdateUI();
}

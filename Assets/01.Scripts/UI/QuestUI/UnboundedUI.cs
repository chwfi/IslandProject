using TMPro;
using UnityEngine.UI;
using UnityEngine;

public abstract class UnboundedUI : MonoBehaviour, IPoolable
{
    protected TextMeshProUGUI _amountText;
    protected Image _icon;

    private void OnEnable() 
    {
        _amountText = GetComponentInChildren<TextMeshProUGUI>();
        _icon = GetComponentInChildren<Image>();
    }  

    public abstract void UpdateUI();

    public void OnTakenFromPool()
    {

    }

    public void OnReturnedToPool()
    {

    }
}

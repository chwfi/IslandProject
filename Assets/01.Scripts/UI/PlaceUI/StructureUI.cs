using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StructureUI : PoolableMono
{
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _priceText;

    private Button _button;

    private void Awake()
    {
        _button = transform.GetComponent<Button>();  
    }

    public void SetUI(PlaceableObjectData data)
    {
        _icon.sprite = data.icon;
        _nameText.text = data.objectName;
        _priceText.text = $"{data.price}";

        _button.onClick.AddListener(() => PlaceManager.Instance.SetPlaceableObject(data));
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class QuestUI : PopupUI
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI _questNameText;
    [SerializeField] private TextMeshProUGUI _goldAmountText;
    [SerializeField] private TextMeshProUGUI _popularityText;

    private QuestInfoUI _ownInfoUI;

    public void SetUI(Quest quest)
    {
        _questNameText.text = $"{quest.QuestName}";
        _goldAmountText.text = $"{quest.Rewards[0].amount}";
        _popularityText.text = $"{quest.Rewards[1].amount}";

        if (_ownInfoUI == null)
        {
            _buttonList.FirstOrDefault().SetSubscription((quest) => SetInfoUI(quest), quest);
        }
    }

    public void SetInfoUI(Quest quest)  
    {
        if (_ownInfoUI == null)
        {
            _ownInfoUI = QuestUIBinder.Instance.SetInfoUI(quest);
            _ownInfoUI.SetUI(quest);
        }
    }
}

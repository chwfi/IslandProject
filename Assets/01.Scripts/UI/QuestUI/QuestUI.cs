using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestUI : PopupUI
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI _questNameText;
    [SerializeField] private TextMeshProUGUI _goldAmountText;
    [SerializeField] private TextMeshProUGUI _popularityText;
    [SerializeField] private GameObject _completionMark;

    private QuestInfoUI _questInfoUI;

    public void SetUI(Quest quest)
    {
        if (quest.IsComplete)
            _completionMark.SetActive(true);

        _questNameText.text = $"{quest.QuestName}";
        _goldAmountText.text = $"{quest.Rewards[0].amount}";
        _popularityText.text = $"{quest.Rewards[1].amount}";

        if (_questInfoUI == null)
        {
            _questInfoUI = quest.SetRegisterInfoUI();
            _buttonList[0].SetSubscription<Quest>((quest) => SetInfoUI(quest));
        }
    }

    public void SetInfoUI(Quest quest)  
    {
        _questInfoUI.AccessUI(true);
    }

    public void UpdateUI(Quest quest)
    {
        if (quest.IsComplete)
            _completionMark.SetActive(true);
    }
}

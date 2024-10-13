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

    public void SetUI(Quest quest)
    {
        _questNameText.text = $"{quest.QuestName}";
        _goldAmountText.text = $"{quest.Rewards[0].amount}";
        _popularityText.text = $"{quest.Rewards[1].amount}";

        _buttonList.FirstOrDefault().SetSubscription((quest) => 
            QuestUIController.Instance.SetUIPair(quest), quest);
    }

    public void DestroyUI(Quest quest)
    {
        PoolManager.Instance.Push(this);

        quest.OnSetUI -= SetUI;
        quest.OnDestroyUI -= DestroyUI;
    }
}

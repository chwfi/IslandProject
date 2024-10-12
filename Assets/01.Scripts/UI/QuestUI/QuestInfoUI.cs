using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Util;

public class QuestInfoUI : PopupUI
{
    [Header("UI Element")]
    [SerializeField] protected TextMeshProUGUI _questNameText;
    [SerializeField] protected TextMeshProUGUI _goldAmountText;
    [SerializeField] protected TextMeshProUGUI _popularityText;

    public virtual void SetUI(Quest quest)
    {
        _questNameText.text = $"{quest.QuestName}";
        _goldAmountText.text = $"{quest.Rewards[0].amount}";
        _popularityText.text = $"{quest.Rewards[1].amount}"; 
    }

    public void SetButton(Quest quest)
    {
        _buttonList.ForEach(x => x.SetSubscription(SetCompletionUI, quest));
    }

    public void SetCompletionUI(Quest quest)
    {
        if (quest.IsCompletable)
        {
            Debug.Log("완");
        }
    }
}
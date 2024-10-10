using System;
using System.Collections;
using System.Collections.Generic;

public class QuestUIController : MonoSingleton<QuestUIController>
{
    public Dictionary<Quest, QuestInfoUI> QuestUIDictionary { get; private set; }

    private void Awake() 
    {
        QuestUIDictionary = new Dictionary<Quest, QuestInfoUI>();    
    }

    public void SetRegisterUI(Quest quest, Action<Quest> action)
    {
        QuestUIBinder.Instance.SetUI(quest);
        action?.Invoke(quest);
    }

    public void SetUIPair(Quest quest)
    {
        if (!QuestUIDictionary.ContainsKey(quest)) // 딕셔너리에 등록된 UI가 아니라면, 새로 생성하고 짝을 맞춰줌
        {
            var newInfoUI = QuestUIBinder.Instance.SetInfoUI(quest);
            newInfoUI.SetUI(quest);

            QuestUIDictionary.Add(quest, newInfoUI);
            AccessInfoUI(quest);
        }
        else // 이미 등록된 것이라면, UI에 Access
        {
            AccessInfoUI(quest);
        }
    }

    private void AccessInfoUI(Quest quest)
    {
        foreach (var kvp in QuestUIDictionary) // Key에 맞는 UI만 켜주고 나머진 꺼줌
        {
            if (kvp.Key == quest)
            {
                kvp.Value.AccessUI(true);
            }
            else
            {
                kvp.Value.AccessUI(false);
            }
        }
    }
}

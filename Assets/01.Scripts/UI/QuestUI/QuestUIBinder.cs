using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestUIBinder : MonoSingleton<QuestUIBinder>
{
    [SerializeField] private QuestUI _questUIPrefab;
    [SerializeField] private QuestInfoUI _questInfoUIPrefab;
    [SerializeField] private Transform _questUITransform;
    [SerializeField] private Transform _questInfoUITransform;

    public QuestUI SetUI(Quest quest)
    {
        QuestUI clone = PoolManager.Instance.Pop("QuestUI") as QuestUI; // 알맞는 위치에 생성
        clone.transform.position = _questUITransform.position;
        quest.OnSetUI += clone.SetUI; // 생성 이후 받은 퀘스트의 이벤트들을 구독해준 후
        quest.OnUpdateUI += clone.UpdateUI;
        return clone; // 반환해줌
    }

    public QuestInfoUI SetInfoUI(Quest quest)
    {
        QuestInfoUI clone = PoolManager.Instance.Pop(_questInfoUIPrefab.name) as QuestInfoUI; // 알맞는 위치에 생성
        clone.transform.position = _questInfoUITransform.position;
        quest.OnSetUI += clone.SetUI; // 생성 이후 받은 퀘스트의 이벤트들을 구독해준 후
        quest.OnUpdateUI += clone.UpdateUI;
        return clone; // 반환해줌 
    }
}

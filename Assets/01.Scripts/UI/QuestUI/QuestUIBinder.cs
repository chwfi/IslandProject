using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class QuestUIBinder : MonoSingleton<QuestUIBinder>
{
    [SerializeField] private QuestUI _questUIPrefab;
    [SerializeField] private QuestInfoUI _questInfoUIPrefab;
    [SerializeField] private Transform _questUITransform;
    [SerializeField] private Transform _questInfoUITransform;

    public void SetUI(Quest quest)
    {
        QuestUI clone = PoolManager.Instance.Pop(_questUIPrefab.name) as QuestUI; // 알맞는 위치에 생성
        SetTransformUtil.SetUIParent(clone.transform, _questUITransform, Vector3.zero);

        quest.OnSetUI += clone.SetUI; // 생성 이후, 주체 퀘스트의 이벤트들을 구독
    }

    public QuestInfoUI SetInfoUI(Quest quest)
    {
        QuestInfoUI clone = PoolManager.Instance.Pop(_questInfoUIPrefab.name) as QuestInfoUI; // 알맞는 위치에 생성
        SetTransformUtil.SetUIParent(clone.transform, _questInfoUITransform, new Vector3(-9, 0, 0));

        quest.OnSetUI += clone.SetUI; // 생성 이후, 주체 퀘스트의 이벤트들을 구독
        clone.SetButton(quest);

        return clone;
    }
}

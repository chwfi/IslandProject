using UnityEngine;
using Util;

public class QuestUIBinder : MonoSingleton<QuestUIBinder>
{
    [Header("Prefabs")]
    [SerializeField] private QuestUI _questUIPrefab;
    [SerializeField] private TaskQuestInfoUI _taskQuestInfoUIPrefab;
    [SerializeField] private TrafficQuestInfoUI _trafficQuestInfoUIPrefab;

    [Header("Instantiate Transfrom")]
    [SerializeField] private Transform _questUITransform;
    [SerializeField] private Transform _questInfoUITransform;

    public void SetUI(Quest quest)
    {
        QuestUI clone = PoolManager.Instance.Pop(_questUIPrefab.name) as QuestUI; // 알맞는 위치에 생성
        SetTransformUtil.SetUIParent(clone.transform, _questUITransform, Vector3.zero, false);

        quest.OnSetUI += clone.SetUI; // 생성 이후, 주체 퀘스트의 이벤트들을 구독
        quest.OnDestroyUI += clone.DestroyUI;
    }

    public QuestInfoUI SetInfoUI(Quest quest)
    {
        // 들어온 퀘스트 타입에 따라 알맞는 퀘스트를 Pool에서 생성
        QuestInfoUI clone = quest switch
        {
            TaskQuest => PoolManager.Instance.Pop(_taskQuestInfoUIPrefab.name) as TaskQuestInfoUI,
            TrafficQuest => PoolManager.Instance.Pop(_trafficQuestInfoUIPrefab.name) as TrafficQuestInfoUI,
            _ => null // 아무것도 아니라면 null 반환
        };
        
        SetTransformUtil.SetUIParent(clone.transform, _questInfoUITransform, new Vector3(-9, 0, 0), false);

        quest.OnSetUI += clone.SetUI; // 생성 이후, 주체 퀘스트의 이벤트들을 구독
        clone.SetButton(quest);

        return clone;
    }
}

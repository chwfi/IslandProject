using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/QuestDatabase")]
public class QuestDatabase : ScriptableObject // 퀘스트들을 담아놓는 데이터베이스. 다른 곳에서 찾아 빼내 쓴다.
{
    [SerializeField] private List<TaskQuest> _taskQuests;
    [SerializeField] private List<TrafficQuest> _trafficQuests;

    public List<TaskQuest> TaskQuests => _taskQuests;
    public List<TrafficQuest> TrafficQuests => _trafficQuests;
}
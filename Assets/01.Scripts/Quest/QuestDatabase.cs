using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/QuestDatabase")]
public class QuestDatabase : ScriptableObject // 퀘스트들을 담아놓는 데이터베이스. 다른 곳에서 찾아 빼내 쓴다.
{
    [SerializeField] private List<Quest> _quests;
    public List<Quest> Quests => _quests;

    public Quest FindQuestBy(int codeName) => _quests.FirstOrDefault(x => x.CodeName == codeName);
}
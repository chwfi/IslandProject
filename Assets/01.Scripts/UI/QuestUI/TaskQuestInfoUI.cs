using UnityEngine;
using Util;

public class TaskQuestInfoUI : QuestInfoUI
{
    [Header("Instantiate")]
    [SerializeField] private UnboundedTaskUI _taskPrefab;
    [SerializeField] private Transform _taskGroupTrm;

    public override void SetUI(Quest quest)
    {
        base.SetUI(quest);

        var taskQuest = (TaskQuest)quest;

        foreach (var task in taskQuest.TaskCloneGroup) // 작업들을 생성해서 UI에 불러옴. 여러개일 수도 있으므로 퀘스트에 접근해 리스트로
        {
            UnboundedTaskUI taskUI = PoolManager.Instance.Take(_taskPrefab.name, _taskGroupTrm) as UnboundedTaskUI;
            SetTransformUtil.SetUIParent(taskUI.transform, _taskGroupTrm, Vector3.zero, true);
            taskUI.SetUp(task);

            taskQuest.OnUpdateUI += taskUI.UpdateUI;

            taskUI.UpdateUI();
        }  
    }
}   

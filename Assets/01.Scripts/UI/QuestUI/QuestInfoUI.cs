using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestInfoUI : PopupUI
{
    [Header("UI Element")]
    [SerializeField] protected TextMeshProUGUI _questNameText;
    [SerializeField] protected TextMeshProUGUI _goldAmountText;
    [SerializeField] protected TextMeshProUGUI _popularityText;
    [SerializeField] protected GameObject _taskPanel;
    [SerializeField] protected GameObject _materialPanel;

    [Header("UnboundedUI Prefab")]
    [SerializeField] protected UnboundedTaskUI _taskPrefab;
    [SerializeField] protected UnboundedMaterialUI _materialPrefab;

    [Header("Prefab Create Transform")]
    [SerializeField] private Transform _taskGroupTrm;
    [SerializeField] private Transform _materialGroupTrm;

    private List<UnboundedUI> _unboundedList = new();

    public void SetUI(Quest quest)
    {
        AccessUI(true);

        if (quest.IsComplete)
        {
            PoolManager.Instance.Push(this);
            return;
        }

        _questNameText.text = $"{quest.QuestName}";
        _goldAmountText.text = $"{quest.Rewards[0].amount}";
        _popularityText.text = $"{quest.Rewards[1].amount}"; 

        if (quest.QuestType == QuestType.TaskQuest)
        {
            _taskPanel.SetActive(true);
            if (_materialPanel.activeInHierarchy) _materialPanel.SetActive(false);
        }
        else
        {
            _materialPanel.SetActive(true);
            if (_taskPanel.activeInHierarchy) _taskPanel.SetActive(false);
        }

        foreach (var task in quest.TaskGroup) // 작업들을 생성해서 UI에 불러옴. 여러개일 수도 있으므로 퀘스트에 접근해 리스트로
        {
            UnboundedTaskUI taskUI = PoolManager.Instance.Pop(_taskPrefab.name) as UnboundedTaskUI;
            taskUI.transform.position = _taskGroupTrm.position;
            taskUI.OwnTask = task;
            _unboundedList.Add(taskUI);
        }      

        foreach (var material in quest.Materials) // 리워드 UI에 띄우는것. 이것도 위 로직과같음
        {
            UnboundedMaterialUI materialUI = PoolManager.Instance.Pop(_materialPrefab.name) as UnboundedMaterialUI;
            materialUI.transform.position = _materialGroupTrm.position;
            materialUI.OwnMaterial = material;
            _unboundedList.Add(materialUI);
        }
    }

    public void UpdateUI(Quest binder)
    {
        if (binder.State == QuestState.Complete)
        {  
            PoolManager.Instance.Push(this);
        }

        foreach (var txt in _unboundedList)
        {
            txt.UpdateUI();
        }
    }
}
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
    [SerializeField] protected GameObject _taskPanel;
    [SerializeField] protected GameObject _materialPanel;

    [Header("UnboundedUI Prefab")]
    [SerializeField] protected UnboundedTaskUI _taskPrefab;
    [SerializeField] protected UnboundedMaterialUI _materialPrefab;

    [Header("Prefab Create Transform")]
    [SerializeField] private Transform _taskGroupTrm;
    [SerializeField] private Transform _materialGroupTrm;

    public void SetUI(Quest quest)
    {
        _questNameText.text = $"{quest.QuestName}";
        _goldAmountText.text = $"{quest.Rewards[0].amount}";
        _popularityText.text = $"{quest.Rewards[1].amount}"; 

        if (quest.QuestType == QuestType.TaskQuest)
        {
            _taskPanel.SetActive(true);
            if (_materialPanel.activeInHierarchy) _materialPanel.SetActive(false);

            foreach (var task in quest.TaskGroup) // 작업들을 생성해서 UI에 불러옴. 여러개일 수도 있으므로 퀘스트에 접근해 리스트로
            {
                UnboundedTaskUI taskUI = PoolManager.Instance.Pop(_taskPrefab.name) as UnboundedTaskUI;
                SetTransformUtil.SetUIParent(taskUI.transform, _taskGroupTrm, Vector3.zero);
                taskUI.SetUp(task);

                quest.OnUpdateUI += taskUI.UpdateUI;

                taskUI.UpdateUI();
            }      
        }
        else
        {
            _materialPanel.SetActive(true);
            if (_taskPanel.activeInHierarchy) _taskPanel.SetActive(false);

            foreach (var matGroup in quest.MaterialGroups) 
            {
                UnboundedMaterialUI materialUI = PoolManager.Instance.Pop(_materialPrefab.name) as UnboundedMaterialUI;
                SetTransformUtil.SetUIParent(materialUI.transform, _materialGroupTrm, Vector3.zero);
                materialUI.SetUp(matGroup);

                MaterialManager.Instance.OnReceivedNotify += materialUI.UpdateUI;

                materialUI.UpdateUI();
            }
        }
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
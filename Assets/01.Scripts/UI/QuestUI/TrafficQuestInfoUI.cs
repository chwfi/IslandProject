using UnityEngine;
using Util;

public class TrafficQuestInfoUI : QuestInfoUI
{
    [Header("Instantiate")]
    [SerializeField] protected UnboundedMaterialUI _materialPrefab;
    [SerializeField] private Transform _materialGroupTrm;

    public override void SetUI(Quest quest)
    {
        base.SetUI(quest);

        var trafficQuest = (TrafficQuest)quest;

        foreach (var matGroup in trafficQuest.MaterialGroups) 
        {
            UnboundedMaterialUI materialUI = PoolManager.Instance.Pop(_materialPrefab.name) as UnboundedMaterialUI;
            SetTransformUtil.SetUIParent(materialUI.transform, _materialGroupTrm, Vector3.zero, true);
            materialUI.SetUp(matGroup);

            MaterialManager.Instance.OnReceivedNotify += materialUI.UpdateUI;

            materialUI.UpdateUI();
        }
    }
}

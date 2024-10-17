using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrafficQuestSaveData
{
    public int codeName;
    public QuestState questState;
    public MaterialGroupData[] materialGroupData;
}

public class MaterialGroupData
{
    public MaterialState materialState;
}

[CreateAssetMenu(menuName = "SO/Quest/TrafficQuest")]  
public class TrafficQuest : Quest, ICloneable<Quest>, IQuestable
{
    [Header("MaterialGroup")]
    [SerializeField] private NeedMaterialGroup[] _materialGroups;

    public IReadOnlyList<NeedMaterialGroup> MaterialGroups => _materialGroups;
    public bool IsAllMaterialGroupMet => _materialGroups.All(x => x.IsComplete);

    public override void OnRegister()
    {
        var materialManager = MaterialManager.Instance;
        materialManager.OnReceivedNotify += OnCheckCompleteMaterial;

        foreach (var material in _materialGroups)
        {
            material.SetOwner(this);
            material.Start();
        }

        base.OnRegister();
    }

    public void OnCheckCompleteMaterial()
    {
        foreach (var matGroup in _materialGroups)
        {
            var matData = MaterialManager.Instance.FindMaterialBy(matGroup.material.CodeName);

            if (matData.MaterialCounter.materialCount >= matGroup.needAmount)
            {
                matGroup.Complete();
            }
        }

        if (IsAllMaterialGroupMet)
        {
            if (_isAutoComplete)
            {
                OnComplete();
            }
            else
            {
                State = QuestState.WaitingForCompletion;
            }
        }
    }

    public override void OnComplete()
    {
        base.OnComplete();

        QuestManager.Instance.ActiveTrafficQuests.Remove(this);

        var materialManager = MaterialManager.Instance;
        materialManager.OnReceivedNotify -= OnCheckCompleteMaterial;
    }

    public TrafficQuestSaveData ToSaveData()
    {
        return new TrafficQuestSaveData
        {
            codeName = _codeName,
            questState = _state,
            materialGroupData = _materialGroups.Select(group => new MaterialGroupData
            {
                materialState = group.MaterialState
            }).ToArray()     
        };
    }

    public void LoadFrom(TrafficQuestSaveData saveData)
    {
        _codeName = saveData.codeName;
        _state = saveData.questState;

        for (int i = 0; i < saveData.materialGroupData.Length; i++)
        {
            _materialGroups[i].MaterialState = saveData.materialGroupData[i].materialState;
        }
    }
}
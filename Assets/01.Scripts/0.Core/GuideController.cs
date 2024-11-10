using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class GuideController : MonoSingleton<GuideController>
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private TextMeshProUGUI _questText;
    [SerializeField] private TextMeshProUGUI _rewardText;
    [SerializeField] private GuideDatabase _database;

    private List<GuideQuest> _guideQuests;
    private GuideQuest _currentGuideQuest;

    private void Start() 
    {
        _guideQuests = new List<GuideQuest>();
    
        for (int i = 0; i < _database.Guides.Count; i++)
        {
            var clone = _database.Guides[i].Clone() as GuideQuest;
            clone.OnRegister();

            _guideQuests.Add(clone);
        }

        _currentGuideQuest = _guideQuests[0];
    }

    public void SetCurrentGuideQuest()
    {
        _guideQuests.Remove(_guideQuests[0]);

        if (_guideQuests.Count <= 0)
        {
            _panel.SetActive(false);
            return;
        }

        _currentGuideQuest = _guideQuests[0];
    }

    private void Update() 
    {
        if (_guideQuests.Count <= 0)
            return;

        _questText.text = $"{_currentGuideQuest.NewTaskClone.Description} {_currentGuideQuest.NewTaskClone.CurrentSuccessValue}/{_currentGuideQuest.NewTaskClone.NeedToSuccessValue}";
        _rewardText.text = $"{_currentGuideQuest.Rewards[0].amount}";
    }
}

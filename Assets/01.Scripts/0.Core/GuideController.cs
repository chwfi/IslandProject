using System.Collections.Generic;
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

    private int _currentGuideIndex = 0;

    private void Start() 
    {
        _guideQuests = new List<GuideQuest>();

        for (int i = 0; i < _database.Guides.Count; i++)
        {
            var clone = _database.Guides[i].Clone() as GuideQuest;
            clone.OnRegister();

            _guideQuests.Add(clone);
        }

        Load();
    }

    public void Load()
    {
        DataManager.Instance.OnLoadFromDatabase<int>("guides", (loadedDatas) =>
        {
            if (loadedDatas != null)
            {
                foreach (var data in loadedDatas)
                {
                    _currentGuideIndex = data;
                    _currentGuideQuest = _guideQuests[_currentGuideIndex];
                }
            }
            else
            {
                Debug.Log("Failed to load data");
            }
        });
    }

    public void SetCurrentGuideQuest()
    {
        _currentGuideIndex++;

        if (_guideQuests.Count <= 0)
        {
            _panel.SetActive(false);
            return;
        }

        _currentGuideQuest = _guideQuests[_currentGuideIndex];
    }

    private void Update() 
    {
        if (_guideQuests.Count <= 0)
            return;

        _questText.text = $"{_currentGuideQuest.NewTaskClone.Description} {_currentGuideQuest.NewTaskClone.CurrentSuccessValue}/{_currentGuideQuest.NewTaskClone.NeedToSuccessValue}";
        _rewardText.text = $"{_currentGuideQuest.Rewards[0].amount}";
    }

    private void OnApplicationQuit() 
    {
        DataManager.Instance.OnSaveData(_currentGuideIndex, "index", "guides"); 
    }
}
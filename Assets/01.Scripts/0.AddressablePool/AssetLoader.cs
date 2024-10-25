using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AssetLoader : MonoBehaviour
{
    [SerializeField] private PoolManagerSO _poolManager;
    [SerializeField] private AssetDBSO _assetDB;
    [SerializeField] private TextMeshProUGUI _loadingText, _subTitleText;

    [SerializeField] private string _nextScene;

    private int _toLoadCount;
    private int _currentLoadedCount;

    public event Action AssetLoadedEvent;
    private bool _isLoadComplete;

    private void Awake() {

        DontDestroyOnLoad(this);
        _isLoadComplete = false;
        _subTitleText.gameObject.SetActive(false);
        
        _currentLoadedCount = 0;

        _assetDB.LoadCountEvent += AddLoadCount;
        _assetDB.LoadMessageEvent += LoadComplete;
        _assetDB.Initialize();

        _poolManager.LoadCountEvent += AddLoadCount;
        _poolManager.LoadMessageEvent += LoadComplete;
        _poolManager.InitializePool(transform);

        AssetLoadedEvent += ()=>{
            _subTitleText.gameObject.SetActive(true);
            _isLoadComplete = true;
        };
    }

    private void Update() 
    {
        if(_isLoadComplete && Input.GetKeyDown(KeyCode.D))
        {
            _isLoadComplete = false;
            SceneManager.LoadScene(_nextScene);
        }
    }

    private void AddLoadCount(int count)
    {
        _toLoadCount += count;
        UpdateLoadText("Ready to load");
    }

    private void LoadComplete(int count, string message)
    {
        _currentLoadedCount += count;
        UpdateLoadText(message);

        if(_currentLoadedCount >= _toLoadCount)
        {
            AssetLoadedEvent?.Invoke();
            Debug.Log("Complete");
        }
    }

    private void UpdateLoadText(string message = null)
    {
        _loadingText.text = $"Loading : {message} - {_currentLoadedCount}/{_toLoadCount}";
    }
}
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadingManager : MonoBehaviour
{
    [SerializeField] private PoolManager poolManager;
    [SerializeField] private string gameSceneName = "GameScene";
    [SerializeField] private TextMeshProUGUI loadingText;
    [SerializeField] private bool showDetailedProgress = true;

    private int totalPoolLists = 5; // MainList, UIList, PlaceableObjectList, EffectList, PlantList
    private int currentPoolList = 0;

    private void Awake()
    {
        if (poolManager != null)
        {
            DontDestroyOnLoad(poolManager.gameObject);
            StartCoroutine(LoadGameScene());
        }
        else
        {
            Debug.LogError("PoolManager is not assigned or found in the scene.");
        }
    }

    private IEnumerator LoadGameScene()
    {
        // PoolManager에 로딩 진행상황 콜백 등록
        poolManager.OnPoolListLoadingStarted += HandlePoolListLoading;
        poolManager.OnPoolItemLoaded += HandlePoolItemLoading;
        
        UpdateLoadingText("오브젝트 풀 초기화 중...");
        poolManager.Init();
        
        yield return new WaitUntil(() => poolManager.IsReady);
        
        UpdateLoadingText("게임 씬 로딩 중...");
        AsyncOperation loadGameScene = SceneManager.LoadSceneAsync(gameSceneName);
        
        while (!loadGameScene.isDone)
        {
            float progress = Mathf.Clamp01(loadGameScene.progress / 0.9f);
            UpdateLoadingText($"게임 씬 로딩 중... {(progress * 100):F0}%");
            yield return null;
        }
    }

    private void HandlePoolListLoading(string listName)
    {
        currentPoolList++;
        if (showDetailedProgress)
        {
            UpdateLoadingText($"오브젝트 풀 초기화 중... ({listName})");
        }
        else
        {
            float progress = (float)currentPoolList / totalPoolLists * 100f;
            UpdateLoadingText($"오브젝트 풀 초기화 중... {progress:F0}%");
        }
    }

    private void HandlePoolItemLoading(string itemName, int current, int total)
    {
        if (showDetailedProgress)
        {
            UpdateLoadingText($"오브젝트 풀 초기화 중...\n{itemName} ({current}/{total})");
        }
    }

    private void UpdateLoadingText(string message)
    {
        if (loadingText != null)
        {
            loadingText.text = message;
        }
    }

    private void OnDestroy()
    {
        if (poolManager != null)
        {
            poolManager.OnPoolListLoadingStarted -= HandlePoolListLoading;
            poolManager.OnPoolItemLoaded -= HandlePoolItemLoading;
        }
    }
}
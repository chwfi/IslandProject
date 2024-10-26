using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    [SerializeField] private PoolManager poolManager;
    [SerializeField] private string gameSceneName = "GameScene";

    private void Awake()
    {
        if (poolManager != null)
        {
            DontDestroyOnLoad(poolManager.gameObject); // PoolManager가 씬 전환 후에도 유지되도록 설정
            StartCoroutine(LoadGameScene());
        }
        else
        {
            Debug.LogError("PoolManager is not assigned or found in the scene.");
        }
    }

    private IEnumerator LoadGameScene()
    {
        // PoolManager의 Init 호출 및 로딩 대기
        poolManager.Init();
        
        // 로딩이 완료될 때까지 대기
        yield return new WaitUntil(() => poolManager.IsReady);
        
        // GameScene 비동기 로드
        AsyncOperation loadGameScene = SceneManager.LoadSceneAsync(gameSceneName);
        
        // 씬 전환 중에도 진행 상황 확인 가능
        while (!loadGameScene.isDone)
        {
            yield return null;
        }
    }
}

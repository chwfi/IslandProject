using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class PolyboxWaterController : MonoBehaviour
{
    [SerializeField]
    Texture2D waveMap;
    [SerializeField]
    Texture2D waveNormalMap;
    [SerializeField]
    Texture2D foamMap;

    public float waveScale = 0.005f;
    public float waveFrequency = 0.005f;
    public float waveFrequency2 = 1.34f;
    public float waveMagnitude = 1.0f;


    private void Awake()
    {
#if UNITY_EDITOR
        var sceneView = UnityEditor.EditorWindow.GetWindow<SceneView>();
        sceneView.sceneViewState.alwaysRefresh = true;
#endif

    }

    void Update()
    {
        Shader.SetGlobalFloat("_PolyboxGlobalWaterScale", 1f/10f);
        
        UpdateNoise();
    }

    private void UpdateNoise()
    {
        Shader.SetGlobalTexture("_PolyboxGlobalWaterWaveMap", waveMap);
        Shader.SetGlobalTexture("_PolyboxGlobalWaterWaveNormalMap", waveNormalMap);
        Shader.SetGlobalTexture("_PolyboxGlobalWaterFoamMap", foamMap);

        Shader.SetGlobalVector("_PolyboxGlobalWaterNoise", new Vector4(
            waveScale * (1f/10f),
            waveFrequency,
            waveFrequency * waveFrequency2,
            waveMagnitude));
    }
}
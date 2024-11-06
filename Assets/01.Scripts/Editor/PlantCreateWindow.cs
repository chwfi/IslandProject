using UnityEditor;
using UnityEngine;

public class PlantCreationWindow : EditorWindow
{
    private GameObject prefabObject; // 사용자 정의할 프리팹
    private ObjectField manager;

    // 창을 표시하는 함수
    public static void ShowWindow(ObjectField manager)
    {
        PlantCreationWindow window = GetWindow<PlantCreationWindow>("Plant Creation");
        window.manager = manager;
    }

    private void OnGUI()
    {
        GUILayout.Label("Plant Creation Settings", EditorStyles.boldLabel);
        GUILayout.Space(15);

        prefabObject = (GameObject)EditorGUILayout.ObjectField("Prefab Object", prefabObject, typeof(GameObject), false);

        GUILayout.Space(15);

        if (GUILayout.Button("Create Plants"))
        {
            if (prefabObject == null)
            {
                Debug.LogWarning("Please assign a prefab object.");
            }
            else
            {
                CreatePlantPositions();
                Close();
            }
        }
    }

    private void CreatePlantPositions()
    {
        if (manager == null || prefabObject == null) return;

        GameObject emptyObject = new GameObject("PlantGroup");
        emptyObject.transform.SetParent(manager.transform.Find("Object").transform, false);
        emptyObject.transform.localPosition = Vector3.zero;

        Vector3[] positions = manager.GetPlantPositions();

        foreach (Vector3 position in positions)
        {
            GameObject plantInstance = Instantiate(prefabObject, emptyObject.transform);
            plantInstance.transform.localPosition = position;
        }

        Debug.Log("Plants created successfully.");
    }
}
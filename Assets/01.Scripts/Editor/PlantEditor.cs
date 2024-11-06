using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ObjectField))]
public class PlantEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUILayout.Space(15);

        if (GUILayout.Button("Open Plant Creation Window", GUILayout.Width(300), GUILayout.Height(50)))
        {
            PlantCreationWindow.ShowWindow((ObjectField)target);
        }
    }
}
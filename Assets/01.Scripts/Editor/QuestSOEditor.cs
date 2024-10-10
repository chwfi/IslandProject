using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(ScriptableObject), true)]
public class QuestSOEditor : Editor
{
    SerializedProperty enumValue;

    void OnEnable()
    {
        if (target is Quest)
            enumValue = serializedObject.FindProperty("_questType");
    }

    public override void OnInspectorGUI()
    {
        if (target is Quest)
        {
            serializedObject.Update();

            QuestType currentQuestType = (QuestType)enumValue.enumValueIndex;

            SerializedProperty property = serializedObject.GetIterator();
            property.NextVisible(true);
            // 루프를 통해 모든 직렬화된 필드들을 돌고, 아래 조건에 따라 특정 필드만 제외하는 기능.

            while (property.NextVisible(false))
            {
                // 퀘스트의 타입이 TaskQuest으로 설정된 상황이라면, _taskGroup 필드는 건너뛰어 인스펙터에 표시하지 않음
                if (property.name == "_taskGroup" && currentQuestType != QuestType.TaskQuest)
                    continue;

                // 퀘스트의 타입이 TrafficQuest으로 설정된 상황이라면, _materialGroups 필드는 건너뛰어 인스펙터에 표시하지 않음
                if (property.name == "_materialGroups" && currentQuestType != QuestType.TrafficQuest)
                    continue;

                EditorGUILayout.PropertyField(property, true);
            }

            serializedObject.ApplyModifiedProperties();
        }
        else
        {
            base.OnInspectorGUI();
        }

        if (target is IQuestable)
        {
            GUILayout.Space(15);

            if (GUILayout.Button("\nSave all quests in editor\n(에디터 상에서 퀘스트 SO의 값을 수정했을 때 사용)\n"))
            {
                QuestSystem.Instance.OnSaveQuestData(); // 클릭 시 동적으로 Firebase DB에 저장
            }
        }
    }
}
#endif
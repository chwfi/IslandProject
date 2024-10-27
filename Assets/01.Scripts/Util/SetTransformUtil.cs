using UnityEngine;

namespace Util
{
    public class SetTransformUtil : MonoBehaviour
    {
        // UI의 부모를 지정해주고 RectTransform을 조정해주는 기능
        // 매개변수로는 주체 UI, 부모로 설정할 UI, 그리고 상세 포지션을 지정할 Vector3값이 있다.
        public static void SetUIParent(Transform subjectUI, Transform parentUI, Vector3 newPosition, bool worldStay = true)
        {
            subjectUI.SetParent(parentUI, worldStay); 
            // 주체로 들어온 UI를 부모로 들어온 UI에 SetParent해준다.
            subjectUI.GetComponent<RectTransform>().anchoredPosition3D = newPosition; 
            // 부모가 설정된 후, 매개변수에 따라 상세 포지션을 정한다.
            subjectUI.localScale = Vector3.one; 
            // 크기 변화 방지를 위해 로컬스케일은 1,1,1로 기본 설정
        }

        public static void SetTransformParent(Transform subject, Transform parent, Vector3 newPosition, bool worldStay = true)
        {
            subject.SetParent(parent, false);
            subject.transform.localPosition = newPosition;
        }
    }
}

using UnityEngine;

namespace Util
{
    public class FindObjectUtil
    {
        //부모에서 특정 타입을 찾기 위해, 그 특정 타입이 존재하는 부모 오브젝트를 찾을때까지 위로 반복 돌리는 함수
        public static T FindParent<T>(GameObject go) where T : Object
        {
            if (go == null) return null; //찾는 기준 오브젝트가 없을때 리턴 null

            Transform currentTransform = go.transform; //찾는 기준 오브젝트의 현재 트랜스폼 저장

            while (currentTransform != null)
            {
                T component = currentTransform.GetComponent<T>(); //현재 Transform에서 T값을 찾음
                if (component != null) //만약 존재한다면
                {
                    return component; //값을 보냄
                }
                currentTransform = currentTransform.parent; //없다면 현재 Transform의 부모를 현재 Transform으로 바꿈

                //부모 오브젝트(currentTransform)이 더이상 없다면 목적 완료한것이므로 while문 종료
            }

            return null;
        }
    }
}
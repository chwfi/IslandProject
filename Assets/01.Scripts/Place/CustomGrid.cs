using UnityEngine;

public class CustomGrid : MonoBehaviour
{
    [SerializeField] private GameObject _target;
    [SerializeField] private PlaceableObject _structure;
    [SerializeField] private float _gridSize;

    private Camera _mainCamera;
    private Vector3 _gridPosition;

    private void Awake() 
    {
        _mainCamera = Camera.main;    
    }

    private void Update() 
    {
        FollowMouse();    
    }

    private void FollowMouse()
    {
        // 마우스 위치에서 레이 생성
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // 레이가 땅이나 특정 오브젝트에 충돌했을 때
        if (Physics.Raycast(ray, out hit))
        {
            // 충돌 지점의 좌표 가져오기
            Vector3 targetPosition = hit.point;

            // Y 값은 고정하고 X와 Z 값만 변경
            targetPosition.y = 0;

            // 오브젝트를 해당 위치로 이동
            _target.transform.position = targetPosition;
        }
    }

    private void LateUpdate() 
    {
        _gridPosition.x = Mathf.Floor(_target.transform.position.x / _gridSize) * _gridSize;    
        //_gridPosition.y = Mathf.Floor(_target.transform.position.y / _gridSize) * _gridSize;
        _gridPosition.z = Mathf.Floor(_target.transform.position.z / _gridSize) * _gridSize;

        _structure.transform.position = _gridPosition;
    }
}
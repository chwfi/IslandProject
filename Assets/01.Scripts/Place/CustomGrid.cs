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
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 targetPosition = hit.point;
            targetPosition.y = 0;
            _target.transform.position = targetPosition;
        }
    }

    private void LateUpdate() 
    {
        _gridPosition.x = Mathf.Floor(_target.transform.position.x / _gridSize) * _gridSize;    
        _gridPosition.z = Mathf.Floor(_target.transform.position.z / _gridSize) * _gridSize;

        _structure.transform.position = _gridPosition;
    }
}
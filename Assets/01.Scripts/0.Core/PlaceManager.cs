using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceManager : MonoSingleton<PlaceManager>
{
    [Header("Database")]
    [SerializeField] private PlaceableDatabase _placeableDatabase;

    [Header("Grid Target")]
    [SerializeField] private GameObject _targetRoot;

    [Header("PlaceableObject Setting")]
    [SerializeField] private Material _positiveMaterial;
    [SerializeField] private Material _negativeMaterial;
    [SerializeField] private float _rayDistance;
    [SerializeField] private LayerMask _layerMask;

    public PlaceableObject CurrentPlaceableObject { get; private set; }

    private Camera _mainCamera;
    private Vector3 _gridPosition;
    private float _gridSize;
    private float _initialHeight;

    private bool _isDragging = false;

    public PlaceableDatabase PlaceableDatabase => _placeableDatabase;
    public Material PositiveMaterial => _positiveMaterial;
    public Material NegativeMaterial => _negativeMaterial;
    public float RayDistance => _rayDistance;
    public LayerMask LayerMask => _layerMask;

    private void Awake() 
    {
        _mainCamera = Camera.main;   
        _initialHeight = _targetRoot.transform.position.y;
    }

    public void InitTargetHeight()
    {
        _targetRoot.transform.position = new Vector3
        (_targetRoot.transform.position.x, _initialHeight, _targetRoot.transform.position.z);
    }

    public void SetPlaceableObject(PlaceableObjectData data)
    {
        PopupUIManager.Instance.MovePopupUI("CreatePanel", new Vector3(0, -375, 0));

        CurrentPlaceableObject = PoolManager.Instance.Take(data.objectName, null) as PlaceableObject;
        _gridSize = CurrentPlaceableObject.transform.localScale.x * 4f;
        CurrentPlaceableObject.transform.position = _targetRoot.transform.position;
        CurrentPlaceableObject.SetPlaceableObject(data);
    }

    public void CancelPlace()
    {
        PoolManager.Instance.Return(CurrentPlaceableObject);
    }

    private void Update() 
    {
        if (CurrentPlaceableObject == null) 
            return;

        HandleInput();
        if (_isDragging)
        {
            Follow();
        }
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit) && hit.transform == 
            CurrentPlaceableObject.ModelObject.transform)
            {
                _isDragging = true;
                CameraController.Instance.canControll = false;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            _isDragging = false;
            CameraController.Instance.canControll = true;
        }
    }

    private void Follow()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 targetPosition = hit.point;
            _targetRoot.transform.position = targetPosition;
        }
    }

    private void LateUpdate() 
    {
        if (CurrentPlaceableObject == null) 
            return;

        _gridPosition.x = Mathf.Floor((_targetRoot.transform.position.x / _gridSize)) * _gridSize + _gridSize / 2f;    
        _gridPosition.z = Mathf.Floor((_targetRoot.transform.position.z / _gridSize)) * _gridSize + _gridSize / 2f;

        CurrentPlaceableObject.transform.position = 
        new Vector3(_gridPosition.x, CurrentPlaceableObject.transform.position.y, _gridPosition.z); 
    }
}

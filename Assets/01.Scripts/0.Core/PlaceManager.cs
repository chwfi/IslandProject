using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceManager : MonoSingleton<PlaceManager>
{
    [Header("Grid Setting")]
    [SerializeField] private float _gridSize;
    [SerializeField] private float _initialHeight;
    [SerializeField] private GameObject _target;

    [Header("PlaceableObject Setting")]
    [SerializeField] private Material _positiveMaterial;
    [SerializeField] private Material _negativeMaterial;
    [SerializeField] private float _rayDistance;
    [SerializeField] private LayerMask _layerMask;

    public PlaceableObject CurrentPlaceableObject { get; private set; }

    private Camera _mainCamera;
    private Vector3 _gridPosition;

    private bool _canPlace = false;
    private bool _isDragging = false;

    public Material PositiveMaterial => _positiveMaterial;
    public Material NegativeMaterial => _negativeMaterial;
    public float RayDistance => _rayDistance;
    public LayerMask LayerMask => _layerMask;

    private void Awake() 
    {
        _mainCamera = Camera.main;   

        _target.transform.position = new Vector3(51.95f, _initialHeight, 0.61f);
    }

    public void SetPlaceableObject(PlaceableObject placeableObject)
    {
        CurrentPlaceableObject = Instantiate(placeableObject);
        CurrentPlaceableObject.transform.position = _target.transform.position;
        CurrentPlaceableObject.SetPlaceableObject();
        
        CameraController.Instance.canControll = false;
        _canPlace = true;
    }

    public void CancelPlace()
    {
        Destroy(CurrentPlaceableObject.gameObject);

        CameraController.Instance.canControll = true;
        _canPlace = false;
    }

    private void Update() 
    {
        if (_canPlace)
        {
            HandleInput();
            if (_isDragging)
            {
                Follow();
            }
        }
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.transform == CurrentPlaceableObject.transform)
            {
                _isDragging = true;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            _isDragging = false;
        }
    }

    private void Follow()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 targetPosition = hit.point;
            targetPosition.y = _initialHeight;
            _target.transform.position = targetPosition;
        }
    }

    private void LateUpdate() 
    {
        if (_canPlace)
        {
            _gridPosition.x = Mathf.Floor((_target.transform.position.x / _gridSize)) * _gridSize + _gridSize / 2f;    
            _gridPosition.z = Mathf.Floor((_target.transform.position.z / _gridSize)) * _gridSize + _gridSize / 2f;

            CurrentPlaceableObject.transform.position = 
            new Vector3(_gridPosition.x, CurrentPlaceableObject.transform.position.y, _gridPosition.z);
        }
    }
}

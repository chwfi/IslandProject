using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableChecker : MonoBehaviour
{
    private MeshRenderer _renderer;
    private BoxCollider _collider;
    private Vector3[] _corners;

    private bool _isOverlap = false;
    private bool _canPlace = false;
    public bool CanPlace => _canPlace;

    private void Start() 
    {
        _collider = transform.GetComponent<BoxCollider>();
        _renderer = transform.GetComponent<MeshRenderer>();
        _corners = new Vector3[4];
    }

    private void Update()
    {
        if (_isOverlap) return;

        bool allHit = PerformRaycastsFromCorners();
        if (allHit)
        {
            ExecuteOnAllRaysHit();
        }
        else
        {
            UnmetRaysHit();
        }
    }

    private bool PerformRaycastsFromCorners()
    {
        PlaceManager manager =  PlaceManager.Instance;
        Bounds bounds = _collider.bounds;

        Vector3 min = bounds.min;
        Vector3 max = bounds.max;

        _corners[0] = new Vector3(min.x, min.y, min.z); // 좌하단 뒷쪽
        _corners[1] = new Vector3(max.x, min.y, min.z); // 우하단 뒷쪽
        _corners[2] = new Vector3(min.x, min.y, max.z); // 좌하단 앞쪽
        _corners[3] = new Vector3(max.x, min.y, max.z); // 우하단 앞쪽

        foreach (Vector3 corner in _corners)
        {
            Ray ray = new Ray(corner, Vector3.down);

            if (!Physics.Raycast(ray, out RaycastHit hit, manager.RayDistance, manager.LayerMask))
            {
                return false;
            }
        }
        return true;
    }

    private void ExecuteOnAllRaysHit()
    {
        _canPlace = true;

        _renderer.material = PlaceManager.Instance.PositiveMaterial;
    }

    private void UnmetRaysHit()
    {
        _canPlace = false;

        _renderer.material = PlaceManager.Instance.NegativeMaterial;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((PlaceManager.Instance.LayerMask & (1 << other.gameObject.layer)) == 0)
        {
            _isOverlap = true;
            UnmetRaysHit();
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        _isOverlap = false;
        ExecuteOnAllRaysHit();
    }
}

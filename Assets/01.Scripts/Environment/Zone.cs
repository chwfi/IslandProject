using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Zone : MonoBehaviour
{
    [SerializeField] private int _expandPrice; 
    private GameObject _camera;

    public int ExpandPrice => _expandPrice;
    public GameObject Camera => _camera;

    private List<MeshRenderer> _childRenderers = new List<MeshRenderer>();
    private MaterialPropertyBlock _propertyBlock;
    private Dictionary<MeshRenderer, Color> _originalColors = new Dictionary<MeshRenderer, Color>();

    private void Awake() 
    {
        _camera = transform.Find("Camera").gameObject;
        _childRenderers.AddRange(GetComponentsInChildren<MeshRenderer>());
        _propertyBlock = new MaterialPropertyBlock();

        foreach (var renderer in _childRenderers)
        {
            renderer.GetPropertyBlock(_propertyBlock);
            Color originalColor;
            originalColor = renderer.sharedMaterial.color; // 기본 머터리얼 색을 저장
            _originalColors[renderer] = originalColor;
        }
    }

    private void OnMouseDown() 
    {
        ZoneManager.Instance.SetZone(this);
    }

    public void SetMaterialProperty(Color color)
    {
        foreach (var renderer in _childRenderers)
        {
            renderer.GetPropertyBlock(_propertyBlock);
            _propertyBlock.SetColor("_Color", color); // 컬러 속성 변경
            renderer.SetPropertyBlock(_propertyBlock);
        }
    }

    public void ResetMaterial()
    {
        foreach (var renderer in _childRenderers)
        {
            if (_originalColors.ContainsKey(renderer))
            {
                renderer.GetPropertyBlock(_propertyBlock);
                _propertyBlock.SetColor("_Color", _originalColors[renderer]); // 원래 색깔로 복원
                renderer.SetPropertyBlock(_propertyBlock);
            }
        }
    }
}

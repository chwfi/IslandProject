using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Zone : MonoBehaviour
{
    [SerializeField] private int _expandPrice; 
    [SerializeField] private GameObject _camera;

    public int ExpandPrice => _expandPrice;

    private List<MeshRenderer> _childRenderers = new List<MeshRenderer>();
    private List<ParticleSystem> _particles = new List<ParticleSystem>();
    private MaterialPropertyBlock _propertyBlock;
    private Dictionary<MeshRenderer, Color> _originalColors = new Dictionary<MeshRenderer, Color>();

    private void Awake() 
    {
        _childRenderers.AddRange(GetComponentsInChildren<MeshRenderer>());
        _particles.AddRange(GetComponentsInChildren<ParticleSystem>());
        _propertyBlock = new MaterialPropertyBlock();

        foreach (var renderer in _childRenderers)
        {
            renderer.GetPropertyBlock(_propertyBlock);
            Color originalColor;
            originalColor = renderer.sharedMaterial.color; // 기본 머터리얼 색을 저장
            _originalColors[renderer] = originalColor;
        }
    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    private void OnMouseDown() 
    {
        if (IsPointerOverUIObject()) return;
        ZoneManager.Instance.SetZone(this);
    }

    public void SetZoneElements(Color color)
    {
        _camera.SetActive(true);

        foreach (var renderer in _childRenderers)
        {
            renderer.GetPropertyBlock(_propertyBlock);
            _propertyBlock.SetColor("_Color", color); // 컬러 속성 변경
            renderer.SetPropertyBlock(_propertyBlock);
        }

        _particles.ForEach(p => p.Play());
    }

    public void DisableZoneElements()
    {
        if (_childRenderers[0] == null) return;

        _camera.SetActive(false);

        foreach (var renderer in _childRenderers)
        {
            if (_originalColors.ContainsKey(renderer))
            {
                renderer.GetPropertyBlock(_propertyBlock);
                _propertyBlock.SetColor("_Color", _originalColors[renderer]); // 원래 색깔로 복원
                renderer.SetPropertyBlock(_propertyBlock);
            }
        }

        _particles.ForEach(p => p.Stop());
    }

    public void DestroyZone()
    {
        ItemManager.Instance.UseCoin(_expandPrice, () => 
        {
            var effect = PoolManager.Instance.Pop("ExplosionEffect");
            effect.transform.position = transform.position;
            _camera.SetActive(false);
            Destroy(this.gameObject);
        }, null);
    }
}

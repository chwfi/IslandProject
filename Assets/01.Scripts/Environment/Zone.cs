using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class Zone : MonoBehaviour
{
    [Header("Price")]
    [SerializeField] private int _expandPrice; 

    [Header("Effect Setting Position")]
    [SerializeField] private Transform[] _effectPlayTransforms;

    public int ExpandPrice => _expandPrice;

    private readonly List<ParticleSystem> _particles = new();

     private List<MeshRenderer> _childRenderers = new List<MeshRenderer>();
    private MaterialPropertyBlock _propertyBlock;
    private Dictionary<MeshRenderer, Color> _originalColors = new Dictionary<MeshRenderer, Color>();

    private void Awake() 
    {
        _particles.AddRange(GetComponentsInChildren<ParticleSystem>());
        _childRenderers.AddRange(GetComponentsInChildren<MeshRenderer>());

        _propertyBlock = new MaterialPropertyBlock();

        foreach (var renderer in _childRenderers)
        {
            renderer.GetPropertyBlock(_propertyBlock);
            Color originalColor;
            originalColor = renderer.sharedMaterial.color;
            _originalColors[renderer] = originalColor;
        }
    }

    void Update()
    {
        if (CameraController.Instance.isMoving)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            if (GameManager.Instance.IsPointerOverUIObject())
                return;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider != null && hit.collider.transform.IsChildOf(transform))
                {
                    ZoneManager.Instance.SetZone(this);
                }
            }
        }
    }

    public void SetZoneElements(Color color)
    {
        foreach (var renderer in _childRenderers)
        {
            renderer.GetPropertyBlock(_propertyBlock);
            _propertyBlock.SetColor("_Color", color);
            renderer.SetPropertyBlock(_propertyBlock);
        }

        _particles.ForEach(p => p.Play());
    }

    public void DisableZoneElements()
    {
        foreach (var renderer in _childRenderers)
        {
            if (_originalColors.ContainsKey(renderer))
            {
                renderer.GetPropertyBlock(_propertyBlock);
                _propertyBlock.SetColor("_Color", _originalColors[renderer]);
                renderer.SetPropertyBlock(_propertyBlock);
            }
        }

        _particles.ForEach(p => p.Stop());
    }

    public void DestroyZone()
    {
        ItemManager.Instance.UseCoin(_expandPrice, () => 
        {
            foreach (var trm in _effectPlayTransforms)
            {
                var effect = PoolManager.Instance.Take("LeafExplosionEffect", null) as EffectPlayer;
                effect.transform.position = trm.position;
            }
            QuestManager.Instance.Report("Expand", 1);
            Destroy(this.gameObject);
        }, null);
    }
}

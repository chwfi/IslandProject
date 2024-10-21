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
    private readonly List<Outline> _outlines = new();

    private void Awake() 
    {
        _particles.AddRange(GetComponentsInChildren<ParticleSystem>());

        Transform[] allChildren = GetComponentsInChildren<Transform>();
        foreach(Transform child in allChildren) 
        {
            var outline = child.gameObject.AddComponent<Outline>();

            outline.enabled = false;
            outline.OutlineColor = ZoneManager.Instance.OutlineColor;
            outline.OutlineWidth = ZoneManager.Instance.OutlineWidth;

            _outlines.Add(outline);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (IsPointerOverUIObject())
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

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    public void SetZoneElements()
    {
        foreach (var outline in _outlines)
        {
            outline.enabled = true;
        }

        _particles.ForEach(p => p.Play());
    }

    public void DisableZoneElements()
    {
        foreach (var outline in _outlines)
        {
            outline.enabled = false;
        }

        _particles.ForEach(p => p.Stop());
    }

    public void DestroyZone()
    {
        ItemManager.Instance.UseCoin(_expandPrice, () => 
        {
            foreach (var trm in _effectPlayTransforms)
            {
                var effect = PoolManager.Instance.Pop("LeafExplosionEffect");
                effect.transform.position = trm.position;
            }
            Destroy(this.gameObject);
        }, null);
    }
}

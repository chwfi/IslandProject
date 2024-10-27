using System.Collections.Generic;
using UnityEngine;
using Util;

public class EffectPlayer : MonoBehaviour, IPoolable
{
    [Header("Option")]
    [SerializeField] private bool _isStartOnEnable;
    [SerializeField] private float _startDelayTime;
    [SerializeField] private float _endTime;

    private List<ParticleSystem> _particles = new List<ParticleSystem>();

    private void Start() 
    {
        _particles.AddRange(transform.GetComponentsInChildren<ParticleSystem>());    
    }

    public void PlayEffect()
    {
        foreach (var particle in _particles)
        {
            particle.Play();
        }
    }

    public void OnTakenFromPool()
    {
        if (_isStartOnEnable)
        {
            CoroutineUtil.CallWaitForSeconds(_startDelayTime, () => PlayEffect());
        }

        CoroutineUtil.CallWaitForSeconds(_endTime, () => PoolManager.Instance.Return(this));
    }

    public void OnReturnedToPool()
    {
        
    }
}
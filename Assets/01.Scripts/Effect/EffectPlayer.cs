using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class EffectPlayer : PoolableMono
{
    [Header("Option")]
    [SerializeField] private bool _isStartOnEnable;
    [SerializeField] private float _endTime;

    private List<ParticleSystem> _particles = new List<ParticleSystem>();

    private void OnEnable() 
    {
        _particles.AddRange(transform.GetComponentsInChildren<ParticleSystem>());

        if (_isStartOnEnable)
        {
            foreach (var particle in _particles)
            {
                particle.Play();
            }
        }

        CoroutineUtil.CallWaitForSeconds(_endTime, () => PoolManager.Instance.Push(this));
    }
}
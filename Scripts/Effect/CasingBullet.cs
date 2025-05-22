using System.Collections;
using System.Collections.Generic;
using ObjectPooling;
using UnityEngine;

public class CasingBullet : PoolableMono
{
    [SerializeField] [Range(0.3f, 1f)] private float _disableParticleTime = 0.5f;
    private ParticleSystem _particle;
    
    private void Awake()
    {
        _particle = GetComponent<ParticleSystem>();
    }

    private void OnEnable()
    {
        _particle.Play();
        StartCoroutine(DisableParticle());
    }

    private IEnumerator DisableParticle()
    {
        yield return new WaitForSeconds(_disableParticleTime);
        _particle.Stop();
        gameObject.SetActive(false);
        PoolManager.Instance.Push(this);
    }

    public override void ResetItem()
    {
        gameObject.SetActive(true);
    }
}

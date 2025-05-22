using System;
using System.Collections;
using System.Collections.Generic;
using ObjectPooling;
using UnityEngine;

public class Bullet : PoolableMono
{
    private TrailRenderer _trail;
    
    public float speed = 300f;
    public float disableBulletTime = 2f;

    private void Awake()
    {
        _trail = GetComponent<TrailRenderer>();
    }

    private void OnEnable()
    {
        StartCoroutine(DisableBullet());
    }

    void Update()
    {
        Vector3 dir =  speed * Time.deltaTime * Vector3.left;
        transform.Translate(dir);
    }

    private IEnumerator DisableBullet()
    {
        yield return new WaitForSeconds(disableBulletTime);
        _trail.Clear();
        gameObject.SetActive(false);
        PoolManager.Instance.Push(this);
    }

    public override void ResetItem()
    {
        gameObject.SetActive(true);
    }
}

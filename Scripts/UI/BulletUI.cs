using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletUI : MonoBehaviour
{
    private const int MAX_BULLET = 30;
    [SerializeField] private Weapon _weapon;
    private List<Image> _bulletList = new List<Image>();
    private int _currentBulletCount = MAX_BULLET;
    private void Awake()
    {
        Image[] bullets = GetComponentsInChildren<Image>();
        for (int i = 0; i < bullets.Length; ++i)
        {
            _bulletList.Add(bullets[i]);
        }
    }

    private void OnEnable()
    {
        _weapon.OnFireEvent += HandleFire;
        _weapon.OnReloadingEvent += HandleReloading;
    }

    private void OnDisable()
    {
        _weapon.OnFireEvent -= HandleFire;
        _weapon.OnReloadingEvent -= HandleReloading;
    }

    private void HandleFire() 
    {
        if (_currentBulletCount < 0) return;
        
        _bulletList[_currentBulletCount - 1].enabled = false;
        _currentBulletCount--;
    }

    private void HandleReloading()
    {
        foreach (Image bullet in _bulletList)
        {
            bullet.enabled = true;
        }

        _currentBulletCount = MAX_BULLET;
    }

}

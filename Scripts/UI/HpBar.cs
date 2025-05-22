using System;
using System.Collections;
using System.Collections.Generic;
using Microlight.MicroBar;
using UnityEngine;

public class HpBar : MonoBehaviour
{
    [SerializeField] private Health _playerHealth;
    private const int MAX_HP = 100;
    private MicroBar _microBar;
    private int _currentHp;

    public int healPower = 25;

    private void Awake()
    {
        _currentHp = MAX_HP;
        _microBar = GetComponent<MicroBar>();
        _microBar.Initialize(MAX_HP);
    }

    void OnEnable()
    {
        _playerHealth.OnHitActionEvent += HandleHitEvent;
    }

    void OnDisable()
    {
        _playerHealth.OnHitActionEvent -= HandleHitEvent;
    }

    public void HealHpBar()
    {
        _currentHp += healPower;
        if (_currentHp >= MAX_HP) _currentHp = MAX_HP;
        _microBar.UpdateBar(_currentHp, false, UpdateAnim.Heal);
    }

    public void HandleHitEvent(int damage)
    {
        _currentHp -= damage;
        if (_currentHp <= 0) _currentHp = 0;
        _microBar.UpdateBar(_currentHp, false, UpdateAnim.Damage);
    }
}

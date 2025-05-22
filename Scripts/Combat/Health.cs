using System;
using ObjectPooling;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour, IDamageable
{
    public UnityEvent OnHitEvent;
    public event Action<int> OnHitActionEvent;
    public UnityEvent OnDeadEvent;

    public ActionData actionData;

    private Agent _owner;
    private PoolAgent _poolOwner;
    private int _currentHealth;
    public int CurrentHealth { get => _currentHealth; }

    public void Initialize(Agent agent)
    {
        _owner = agent;
        actionData = new ActionData();
        _currentHealth = _owner.Stat.maxHealth.GetValue(); //최대체력으로 셋팅
    }

    public void Initialize(PoolAgent agent)
    {
        _poolOwner = agent;
        actionData = new ActionData();
        _currentHealth = _poolOwner.Stat.maxHealth.GetValue(); //최대체력으로 셋팅
    }

    public void ApplyDamage(int damage, Vector3 hitPoint, Vector3 normal, float knockbackPower, Agent dealer, DamageType damageType, bool check)
    {
        if (_poolOwner.isDead) return;

        Vector3 textPosition = hitPoint + new Vector3(0, 1f, 0);

        if (_poolOwner.Stat.CanEvasion())
        {
            return;
        }

        actionData.hitNormal = normal;
        actionData.hitPoint = hitPoint;
        actionData.lastDamageType = damageType;

        if (knockbackPower > 0)
        {
            ApplyKnockback(normal * -knockbackPower, true);
        }

        actionData.isCritical = dealer.Stat.IsCritical(ref damage);
        damage = _poolOwner.Stat.ArmoredDamage(damage); //아머수치 적용해서 데미지 계산

        _currentHealth = Mathf.Clamp(
                _currentHealth - damage, 0, _poolOwner.Stat.maxHealth.GetValue());
        OnHitEvent?.Invoke();
        OnHitActionEvent?.Invoke(damage);

        if (_currentHealth <= 0)
        {
            OnDeadEvent?.Invoke();
        }
    }

    public void ApplyDamage(int damage, Vector3 hitPoint, Vector3 normal, float knockbackPower, Agent dealer, DamageType damageType)
    {
        if (_owner.isDead) return;

        Vector3 textPosition = hitPoint + new Vector3(0, 1f, 0);

        if (_owner.Stat.CanEvasion())
        {
            return;
        }

        actionData.hitNormal = normal;
        actionData.hitPoint = hitPoint;
        actionData.lastDamageType = damageType;

        if (knockbackPower > 0)
        {
            ApplyKnockback(normal * -knockbackPower);
        }

        actionData.isCritical = dealer.Stat.IsCritical(ref damage);
        damage = _owner.Stat.ArmoredDamage(damage); //아머수치 적용해서 데미지 계산

        _currentHealth = Mathf.Clamp(
                _currentHealth - damage, 0, _owner.Stat.maxHealth.GetValue());
        OnHitEvent?.Invoke();
        OnHitActionEvent?.Invoke(damage);

        if (_currentHealth <= 0)
        {
            OnDeadEvent?.Invoke();
        }
    }

    public void ApplyDamage(int damage, Vector3 hitPoint, Vector3 normal, float knockbackPower, PoolAgent dealer, DamageType damageType)
    {
        if (_owner.isDead) return;

        Vector3 textPosition = hitPoint + new Vector3(0, 1f, 0);

        if (_owner.Stat.CanEvasion())
        {
            return;
        }

        actionData.hitNormal = normal;
        actionData.hitPoint = hitPoint;
        actionData.lastDamageType = damageType;

        if (knockbackPower > 0)
        {
            ApplyKnockback(normal * -knockbackPower);
        }

        actionData.isCritical = dealer.Stat.IsCritical(ref damage);
        damage = _owner.Stat.ArmoredDamage(damage); //아머수치 적용해서 데미지 계산

        _currentHealth = Mathf.Clamp(
                _currentHealth - damage, 0, _owner.Stat.maxHealth.GetValue());
        OnHitEvent?.Invoke();
        OnHitActionEvent?.Invoke(damage);

        if (_currentHealth <= 0)
        {
            OnDeadEvent?.Invoke();
        }
    }

    private void ApplyKnockback(Vector3 force)
    {
        _owner.MovementCompo.GetKnockback(force);
    }

    private void ApplyKnockback(Vector3 force, bool checke)
    {
        _poolOwner.MovementCompo.GetKnockback(force);
    }
}

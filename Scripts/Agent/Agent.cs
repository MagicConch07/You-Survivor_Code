using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public abstract class Agent : MonoBehaviour
{
    #region component list section
    public Animator AnimatorCompo { get; protected set; }
    public IMovement MovementCompo { get; protected set; }
    public AgentVFX VFXCompo { get; protected set; }
    public DamageCaster DamageCasterCompo { get; protected set; }
    public Health HealthCompo { get; protected set; }
    #endregion

    [field: SerializeField] public AgentStat Stat { get; protected set; }

    public bool CanStateChangeable { get; protected set; } = true;

    public bool isDead;

    protected virtual void Awake()
    {
        Transform visualTrm = transform.Find("Visual");
        AnimatorCompo = visualTrm.GetComponent<Animator>();
        MovementCompo = GetComponent<IMovement>();
        MovementCompo.Initialize(this);

        Transform damageTrm = transform.Find("DamageCaster");
        if (damageTrm != null)
        {
            DamageCasterCompo = damageTrm.GetComponent<DamageCaster>();
            DamageCasterCompo.InitCaster(this);
        }

        Stat = Instantiate(Stat);
        Stat.SetOwner(this);

        HealthCompo = GetComponent<Health>();
        HealthCompo.Initialize(this);
    }

    public Coroutine StartDelayCallback(float time, Action Callback)
    {
        return StartCoroutine(DelayCoroutine(time, Callback));
    }

    protected IEnumerator DelayCoroutine(float time, Action Callback)
    {
        yield return new WaitForSeconds(time);
        Callback?.Invoke();
    }

    public virtual void Attack()
    {

    }

    public abstract void SetDead();
}

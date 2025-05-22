using System;
using TMPro;
using UnityEngine;

public enum CommonStateEnum
{
    Idle,
    Battle,  //추적 상태
    Attack,
    Damage,
    Dead
}

public class CommonEnemy : Enemy
{

    public EnemyStateMachine<CommonStateEnum> StateMachine { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        StateMachine = new EnemyStateMachine<CommonStateEnum>();

        foreach (CommonStateEnum stateEnum in Enum.GetValues(typeof(CommonStateEnum)))
        {
            string typeName = stateEnum.ToString();
            Type t = Type.GetType($"Common{typeName}State");

            try
            {
                EnemyState<CommonStateEnum> state =
                    Activator.CreateInstance(t, this, StateMachine, typeName) as EnemyState<CommonStateEnum>;
                StateMachine.AddState(stateEnum, state);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Enemy Hammer : no State found [ {typeName} ] - {ex.Message}");
            }
        }
    }

    void OnEnable()
    {
        StateMachine.Initialize(CommonStateEnum.Idle, this);
    }

    private void Update()
    {
        StateMachine.CurrentState.UpdateState();
    }

    public void Damage()
    {
        StateMachine.ChangeState(CommonStateEnum.Damage);
    }

    public override void Attack()
    {
        DamageCasterCompo.CastDamage(777);
    }

    public override void AnimationEndTrigger()
    {
        StateMachine.CurrentState.AnimationFinishTrigger();
    }

    public override void SetDead()
    {
        StateMachine.ChangeState(CommonStateEnum.Dead, true);
        _collider.enabled = false;
        _rigidbody.useGravity = false;
    }

    public override void ResetItem()
    {
        gameObject.SetActive(true);
    }
}

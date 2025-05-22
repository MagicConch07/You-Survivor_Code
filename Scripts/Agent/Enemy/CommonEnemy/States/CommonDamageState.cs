using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class CommonDamageState : EnemyState<CommonStateEnum>
{
    public CommonDamageState(Enemy enemyBase, EnemyStateMachine<CommonStateEnum> stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _enemyBase.AnimatorCompo.Play(_animBoolHash, -1, 0f);
        _enemyBase.MovementCompo.StopImmediately();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (_endTriggerCalled)
            _stateMachine.ChangeState(CommonStateEnum.Idle);
    }
}

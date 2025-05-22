using UnityEngine;

public class CommonAttackState : EnemyState<CommonStateEnum>
{
    private EnemyMovement movementCompo;
    public CommonAttackState(Enemy enemyBase, EnemyStateMachine<CommonStateEnum> stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        movementCompo = _enemyBase.MovementCompo as EnemyMovement;
    }

    public override void Enter()
    {
        base.Enter();
        _enemyBase.MovementCompo.StopImmediately();
        movementCompo.LookToTarget(_enemyBase.targetTrm.position);
    }


    public override void Exit()
    {
        _enemyBase.lastAttackTime = Time.time;
        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (_endTriggerCalled)
            _stateMachine.ChangeState(CommonStateEnum.Battle);
    }
}

using UnityEngine;

public class CommonIdleState : EnemyState<CommonStateEnum>
{
    private readonly int _randomIdleHash = Animator.StringToHash("RamdomIdle");
    private readonly int _idleAction2 = Animator.StringToHash("Idle_Action2");
    private bool _isMethod = false;
    public CommonIdleState(Enemy enemyBase, EnemyStateMachine<CommonStateEnum> stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
    }

    public void RandomAnimation()
    {
        float rand = Random.Range(0f, 1f);
        _enemyBase.AnimatorCompo.SetFloat(_randomIdleHash, rand);
        if (rand > 0.5f)
            _enemyBase.AnimatorCompo.SetBool(_idleAction2, true);
        else
            _enemyBase.AnimatorCompo.SetBool(_idleAction2, false);

        _isMethod = false;
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (_endTriggerCalled)
        {
            _enemyBase.AnimatorCompo.SetBool(_idleAction2, false);
            _endTriggerCalled = false;
        }

        if (_isMethod == false)
        {
            _isMethod = true;
            _enemyBase.StartDelayCallback(3f, RandomAnimation);
        }

        Collider target = _enemyBase.IsPlayerDetected();
        if (target == null) return; //주변에 플레이어가 없으면 아무것도 안함.

        Vector3 direction = target.transform.position - _enemyBase.transform.position;
        direction.y = 0;

        //플레이어 발견했고 그 사이에 장애물도 없다.
        if (_enemyBase.IsObstacleInLine(direction.magnitude, direction.normalized) == false)
        {
            _enemyBase.targetTrm = target.transform;
            _stateMachine.ChangeState(CommonStateEnum.Battle); //전투상태로 전환
        }
    }

    public override void Exit()
    {
        _enemyBase.AnimatorCompo.SetBool(_idleAction2, false);
        _enemyBase.AnimatorCompo.SetFloat(_randomIdleHash, 0f);
        base.Exit();
    }


}

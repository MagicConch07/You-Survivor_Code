using ObjectPooling;
using System.Collections;
using UnityEngine;

public class CommonDeadState : EnemyState<CommonStateEnum>
{
    private bool _isDissolve = false;
    private readonly int _dissolveHeightHash = Shader.PropertyToID("_DissolveHeight");

    private int _deadbodyLayer = LayerMask.NameToLayer("DeadBody");


    public CommonDeadState(Enemy enemyBase, EnemyStateMachine<CommonStateEnum> stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _enemyBase.gameObject.layer = _deadbodyLayer;

        Vector3 dropDirection = _enemyBase.HealthCompo.actionData.hitNormal * -1;
        Vector3 realDir = Quaternion.Euler(0, Random.Range(-30f, 30f), 0) * dropDirection;
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (_endTriggerCalled && _isDissolve == false)
        {
            _isDissolve = true;
            _enemyBase.StartCoroutine(StartDissolveCoroutine());
        }

    }

    private IEnumerator StartDissolveCoroutine()
    {
        float currentTime = 0;
        float totalTime = 2f;
        Material mat = _enemyBase.meshRenderer.material;
        while (currentTime / totalTime <= 1)
        {
            float value = Mathf.Lerp(2, -2, currentTime / totalTime);
            mat.SetFloat(_dissolveHeightHash, value);
            yield return null;
            currentTime += Time.deltaTime;
        }
        yield return new WaitForSeconds(0.2f);
        _enemyBase.gameObject.SetActive(false);
        PoolManager.Instance.Push(_enemyBase);
    }
}

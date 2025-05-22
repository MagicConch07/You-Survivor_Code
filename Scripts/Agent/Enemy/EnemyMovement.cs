using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour, IMovement
{
    public Vector3 Velocity => _navAgent.velocity;
    public bool IsGround { get; }

    [SerializeField] private float _knockbackThreshold;
    [SerializeField] private float _maxKnockbackTime;

    public NavMeshAgent NavAgent => _navAgent;

    private Enemy _enemy;
    private CommonEnemy _commonEnemy;
    private NavMeshAgent _navAgent;
    private Rigidbody _rbCompo;

    private float _knockbackStartTime;
    private bool _isKnockback;

    public void Initialize(PoolAgent agent)
    {
        _enemy = agent as Enemy;
        _commonEnemy = GetComponent<CommonEnemy>();
        _navAgent = GetComponent<NavMeshAgent>();
        _navAgent.speed = _enemy.moveSpeed;
        _rbCompo = GetComponent<Rigidbody>();
    }

    public void SetDestination(Vector3 destination)
    {
        if (_navAgent.enabled == false) return;
        _navAgent.isStopped = false;
        _navAgent.SetDestination(destination);
    }

    public void StopImmediately()
    {
        if (_navAgent.enabled == false) return;
        _navAgent.isStopped = true;
    }

    public void LookToTarget(Vector3 targetPos)
    {
        Vector3 toward = targetPos - transform.position;
        toward.y = 0;
        transform.rotation = Quaternion.LookRotation(toward);
    }


    public void GetKnockback(Vector3 force)
    {
        _commonEnemy.Damage();
        StartCoroutine(ApplyKnockbackCoroutine(force));
    }

    private IEnumerator ApplyKnockbackCoroutine(Vector3 force)
    {
        StopImmediately();

        _rbCompo.isKinematic = false;
        _rbCompo.AddForce(force, ForceMode.Impulse);
        _knockbackStartTime = Time.time;

        if (_isKnockback)
            yield break;

        _isKnockback = true;
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();

        yield return new WaitUntil(
            () => _rbCompo.velocity.magnitude < _knockbackThreshold ||
                    Time.time > _knockbackStartTime + _maxKnockbackTime);

        DisableRigidbody();

        _isKnockback = false;

        yield return null;
    }

    private void DisableAgent()
    {
        _navAgent.enabled = false;
    }

    private void DisableRigidbody()
    {
        _rbCompo.velocity = Vector3.zero;
        _rbCompo.angularVelocity = Vector3.zero;
        _rbCompo.isKinematic = true;
    }
}

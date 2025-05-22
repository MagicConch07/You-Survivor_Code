using UnityEngine;

public abstract class Enemy : PoolAgent
{
    [Header("Common settings")]
    public float moveSpeed;
    public float battleTime;
    public bool isActive;

    protected CapsuleCollider _collider;
    protected Rigidbody _rigidbody;

    [SerializeField] protected LayerMask _whatIsPlayer;
    [SerializeField] protected LayerMask _whatIsObstacle;

    [Header("Attack Settings")]
    public float runAwayDistance;
    public float attackDistance;
    public float attackCooldown;
    [SerializeField] protected int _maxCheckEnemy = 1;
    [HideInInspector] public float lastAttackTime;
    [HideInInspector] public Transform targetTrm;
    protected Collider[] _enemyCheckColliders;

    public SkinnedMeshRenderer meshRenderer;

    protected override void Awake()
    {
        base.Awake();
        _enemyCheckColliders = new Collider[_maxCheckEnemy];
        _collider = GetComponent<CapsuleCollider>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        moveSpeed = Random.Range(5.5f, 18f);
    }

    public virtual Collider IsPlayerDetected()
    {
        int cnt = Physics.OverlapSphereNonAlloc(transform.position, runAwayDistance, _enemyCheckColliders, _whatIsPlayer);
        return cnt >= 1 ? _enemyCheckColliders[0] : null;
    }

    public virtual bool IsObstacleInLine(float distance, Vector3 direction)
    {
        return Physics.Raycast(transform.position, direction, distance, _whatIsObstacle);
    }


    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, runAwayDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
        Gizmos.color = Color.white;
    }

    public abstract void AnimationEndTrigger();
}

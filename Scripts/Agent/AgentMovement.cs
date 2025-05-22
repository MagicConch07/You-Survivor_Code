using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class AgentMovement : MonoBehaviour, IMovement
{
    [SerializeField] private InputReader _inputReader;

    [Header("Ground Layer Settings")]
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _groundRayDistance;

    protected Rigidbody _myRigidbody;
    private Agent _agent;

    public float moveSpeed = 10f;
    public float sprintSpeed = 12f;
    public float jumpPower = 5f;
    private Vector3 _velocity;
    public Vector3 Velocity => _velocity;
    public bool IsGround { get; private set; }
    public float groundRayDistance = 1.3f;

    private float originSpeed;
    private Vector2 _movementInput;

    public void Initialize(Agent agent)
    {
        _myRigidbody = GetComponent<Rigidbody>();
        _agent = agent;
        originSpeed = moveSpeed;
    }

    private void OnEnable()
    {
        _inputReader.OnJumpEvent += HandleJump;
        _inputReader.OnSprintEvent += HandleSprint;
    }

    private void OnDisable()
    {
        _inputReader.OnJumpEvent -= HandleJump;
        _inputReader.OnSprintEvent -= HandleSprint;
    }

    private void HandleSprint(bool isPress)
    {
        if (isPress)
            moveSpeed = sprintSpeed;
        else
            moveSpeed = originSpeed;
    }

    private void HandleJump()
    {
        // Nothing
    }


    private void Update()
    {
        IsGround = GroundCheck();
        Gravity();
    }

    public float gravityPower = 3;
    private void Gravity()
    {
        _myRigidbody.AddForce(new Vector3(0, -9.81f * gravityPower * Time.deltaTime, 0), ForceMode.Impulse);
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        _movementInput = _inputReader.PlayerActionsInstance.Movement.ReadValue<Vector2>();

        _velocity = new Vector3(_movementInput.x, 0, _movementInput.y) * moveSpeed;
        _velocity = transform.TransformDirection(_velocity);

        _myRigidbody.velocity = new Vector3(_velocity.x, _myRigidbody.velocity.y, _velocity.z);
    }

    private bool GroundCheck()
    {
        return Physics.Raycast(transform.position, Vector3.down * groundRayDistance, _groundRayDistance, _groundLayer);
    }

    public void StopImmediately()
    {
        _velocity = Vector3.zero;
    }

    public void SetMovement(Vector3 movement, bool isRotation = true)
    {
        // Noting
    }

    public void SetDestination(Vector3 destination)
    {
    }

    public void GetKnockback(Vector3 force)
    {
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, Vector3.down);
    }
#endif
}

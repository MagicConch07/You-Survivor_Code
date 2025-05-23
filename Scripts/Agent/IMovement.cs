using UnityEngine;

public interface IMovement
{
    public Vector3 Velocity { get; }
    public bool IsGround { get; }
    public void Initialize(Agent agent);
    public void Initialize(PoolAgent agent);
    public void SetMovement(Vector3 movement, bool isRotation = true);
    public void StopImmediately();
    public void SetDestination(Vector3 destination);


    public void GetKnockback(Vector3 force);
}

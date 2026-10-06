using UnityEngine;


public class CharapterMoveController : MonoBehaviour
{
    protected IMovable MoveTarget;

    public void SetTarget(IMovable movable)
    {
        MoveTarget = movable;
        movable.MoveSpeed = 0;
        movable.IsMoving = false;
    }

    protected void BeginMove()
    {
        MoveTarget.MoveSpeed = 0;
        MoveTarget.IsMoving = true;
    }
    
    protected void StopMove()
    {
        MoveTarget.MoveSpeed = 0;
        MoveTarget.IsMoving = false;
    }
    
    protected void BeginRotate(float direction)
    {
        MoveTarget.RotationSpeed = 0;
        MoveTarget.RotationDirection = direction;
    }
    
    protected void StopRotate()
    {
        MoveTarget.RotationSpeed = 0;
        MoveTarget.RotationDirection = 0;
    }

    protected void Update()
    {
        if (MoveTarget == null || MoveTarget.IsDead)
            return;
        
        if (MoveTarget is {IsMoving: true})
        {
            MoveTarget.MoveSpeed = Mathf.Lerp(MoveTarget.MoveSpeed, MoveTarget.MaxMoveSpeed, 20 * Time.deltaTime);
            MoveTarget.Position += MoveTarget.Orientation.normalized * (MoveTarget.MoveSpeed * 50 * Time.deltaTime);
        }
        
        if (MoveTarget is { IsRotating : true})
        {
            Vector3 r = MoveTarget.Rotation;
            MoveTarget.RotationSpeed = Mathf.Lerp(MoveTarget.RotationSpeed, MoveTarget.MaxRotationSpeed, 1 * Time.deltaTime);
            r.z += MoveTarget.RotationSpeed * MoveTarget.RotationDirection * Time.deltaTime;
            MoveTarget.Rotation = r;
        }
    }
}
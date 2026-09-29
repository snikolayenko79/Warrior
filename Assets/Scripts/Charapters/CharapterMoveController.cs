using UnityEngine;

public class CharapterMoveController : MonoBehaviour
{
    protected IMovable MoveTarget;

    public void SetTarget(IMovable movable)
    {
        MoveTarget = movable;
    }

    protected void Update()
    {
        if (MoveTarget is {IsMoving: true})
        {
            MoveTarget.Position += MoveTarget.Orientation.normalized * (MoveTarget.MoveSpeed * Time.deltaTime);
        }
        
        if (MoveTarget is { IsRotating : true})
        {
            Vector3 r = MoveTarget.Rotation;
            r.z += MoveTarget.RotationSpeed * MoveTarget.RotationDirection * Time.deltaTime;
            MoveTarget.Rotation = r;
        }
        
    }
}
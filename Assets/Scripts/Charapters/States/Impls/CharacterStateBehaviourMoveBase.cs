using UnityEngine;

public class CharacterStateBehaviourMoveBase : MovableStateBehaviourBase
{
    public override void StateStart(ICharapterState characterState)
    {
    }
    
    protected override void MovableUpdate(IMovable movable)
    {
        if (movable.IsMoving)
            Move(movable);
        
        if (movable.IsRotating)
            Rotate(movable);
    }

    public override void StateEnd(ICharapterState characterState)
    {
    }
    
    public void BeginMove(IMovable  movable)
    {
        movable.MoveSpeed = 0;
        movable.IsMoving = true;
    }
    
    public void StopMove(IMovable  movable)
    {
        movable.MoveSpeed = 0;
        movable.IsMoving = false;
    }
    
    protected void Move(IMovable movable)
    {
        movable.MoveSpeed = Mathf.Lerp(movable.MoveSpeed, movable.MaxMoveSpeed, 20 * Time.deltaTime);
        movable.Position += movable.Orientation.normalized * (movable.MoveSpeed * 50 * Time.deltaTime);
    }
    
    public void BeginRotate(IMovable  movable, float direction)
    {
        movable.RotationSpeed = 0;
        movable.RotationDirection = direction;
    }
    
    public void StopRotate(IMovable  movable)
    {
        movable.RotationSpeed = 0;
        movable.RotationDirection = 0;
    }
    
    protected void Rotate(IMovable movable)
    {
        Vector3 r = movable.Rotation;
        movable.RotationSpeed = Mathf.Lerp(movable.RotationSpeed, movable.MaxRotationSpeed, 1 * Time.deltaTime);
        r.z += movable.RotationSpeed * movable.RotationDirection * Time.deltaTime;
        movable.Rotation = r;
    }
}
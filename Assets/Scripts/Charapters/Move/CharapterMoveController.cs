using UnityEngine;
using System.Collections.Generic;

public class CharapterMoveController : MonoBehaviour, ICharacterMoveController
{
    protected List<IMovable> MovableEntities = new List<IMovable>();
    
    public void RegisterEntity(IMovable movable) => MovableEntities.Add(movable);
    public void UnregisterEntity(IMovable movable) => MovableEntities.Remove(movable);

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

    public void Move(IMovable movable)
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
    
    public void Rotate(IMovable movable)
    {
        Vector3 r = movable.Rotation;
        movable.RotationSpeed = Mathf.Lerp(movable.RotationSpeed, movable.MaxRotationSpeed, 1 * Time.deltaTime);
        r.z += movable.RotationSpeed * movable.RotationDirection * Time.deltaTime;
        movable.Rotation = r;
    }

    public void UpdateMovement(IMovable movable)
    {
        if (movable is {IsMoving: true})
        {
            Move(movable);
        }
        
        if (movable is { IsRotating : true})
        {
            Rotate(movable);
        }
    }
}
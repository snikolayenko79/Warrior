using UnityEngine;
using System.Collections.Generic;

public class CharapterMoveController : MonoBehaviour
{
    protected List<IMovable> Movables = new List<IMovable>();

    public void AddMovable(IMovable movable)
    {
        if (!Movables.Contains(movable))
            Movables.Add(movable);
        
        movable.MoveSpeed = 0;
        movable.IsMoving = false;
    }

    protected void BeginMove(IMovable  movable)
    {
        movable.MoveSpeed = 0;
        movable.IsMoving = true;
    }
    
    protected void StopMove(IMovable  movable)
    {
        movable.MoveSpeed = 0;
        movable.IsMoving = false;
    }

    protected void Move(IMovable movable)
    {
        movable.MoveSpeed = Mathf.Lerp(movable.MoveSpeed, movable.MaxMoveSpeed, 20 * Time.deltaTime);
        movable.Position += movable.Orientation.normalized * (movable.MoveSpeed * 50 * Time.deltaTime);
    }
    
    protected void BeginRotate(IMovable  movable, float direction)
    {
        movable.RotationSpeed = 0;
        movable.RotationDirection = direction;
    }
    
    protected void StopRotate(IMovable  movable)
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

    protected void Update()
    {
        foreach (var movable in Movables)
        {
            if (movable == null || movable.IsDead)
                continue;
        
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
}
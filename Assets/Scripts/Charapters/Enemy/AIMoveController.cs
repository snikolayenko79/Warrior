using UnityEngine;

public class AIMoveController : CharapterMoveController
{
    private IAttackable _attackable;
    
    public void SetAttackable(IAttackable attackable)
    {
        _attackable = attackable;
    }
    
    protected new void Update()
    {
        for (int i = MovableEntities.Count - 1; i >= 0; i--)
        {
            IMovable movable = MovableEntities[i];
            
            if (movable == null || movable.IsDead)
            {
                MovableEntities.RemoveAt(i);
                continue;
            }

            if (movable.CurrentPath == null)
            {
                StopMove(movable);
                continue;
            }

            Vector3? targetPosition = movable.CurrentPath.GetCurrentTarget();
            if (!targetPosition.HasValue)
            {
                StopMove(movable);
                continue;
            }
        
            if (!movable.IsMoving)
                BeginMove(movable);
        
            if (Vector3.Distance(movable.Position, targetPosition.Value) <= 0.5f)
            {
                // Дошли до точки на маршруте
                movable.CurrentPath.NextTarget();
            }
        
            // Вращение в направлении точки на маршруте
            RotateTowards(movable, targetPosition.Value);
            
            Move(movable);
            Rotate(movable);
        }
    }

    private void RotateTowards(IMovable target, Vector3 targetPos)
    {
        Vector3 directionToPlayer = targetPos - target.Position;
        directionToPlayer.y = 0;

        float forwardDot = Vector3.Dot(target.Orientation, directionToPlayer.normalized);
        float sideDot = Vector3.Dot(target.Right, directionToPlayer.normalized);
        float turnInput = Mathf.Clamp(sideDot, -1f, 1f);
        float angle = Mathf.Acos(forwardDot) * Mathf.Rad2Deg;
        if (angle > 1)
        {
            //Debug.Log(angle);
            // Vector3 r = MoveTarget.Rotation;
            // r.y += angle * turnInput;
            // MoveTarget.Rotation = Vector3.Lerp(MoveTarget.Rotation, r, Time.deltaTime * MoveTarget.MaxRotationSpeed);
            float direction = Mathf.Sign(angle * turnInput);
            if (!target.IsRotating || target.RotationDirection != direction)
                BeginRotate(target, direction);
        }
        else
        {
            if (target.IsRotating)
                StopRotate(target);
        }
    }
}
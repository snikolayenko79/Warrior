using UnityEngine;

public class BehaviourFollowPath : MovableBehaviourBase
{
    public override void StateStart(ICharapterState characterState)
    {
    }

    protected override void MovableUpdate(IMovable movable)
    {
        if (movable.IsDead)
            return;
        
        if (movable.CurrentPath == null)
        {
            movable.IsMoving = false;
            movable.RotationDirection = 0;
            return;
        }

        Vector3? targetPosition = movable.CurrentPath.GetCurrentTarget();
        if (!targetPosition.HasValue)
        {
            movable.IsMoving = false;
            movable.RotationDirection = 0;
            return;
        }

        if (!movable.IsMoving)
        {
            movable.IsMoving = true;
            movable.MoveSpeed = 0;
        }
        
        if (Vector3.Distance(movable.Position, targetPosition.Value) <= 0.5f)
        {
            // Дошли до точки на маршруте
            movable.CurrentPath.NextTarget();
        }
        
        // Вращение в направлении точки на маршруте
        RotateTowards(movable, targetPosition.Value);
    }

    public override void StateEnd(ICharapterState characterState)
    {
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
            {
                target.RotationSpeed = 0;
                target.RotationDirection = direction;
            }
        }
        else
        {
            if (target.IsRotating)
            {
                target.RotationDirection = 0;
            }
        }
    }
}
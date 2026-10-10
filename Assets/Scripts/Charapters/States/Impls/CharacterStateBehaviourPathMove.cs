using UnityEngine;

public class CharacterStateBehaviourPathMove : CharacterStateBehaviourMoveBase
{
    public override void StateStart(ICharapterState characterState)
    {
    }

    protected override void MovableUpdate(IMovable movable)
    {
        if (movable.CurrentPath == null)
        {
            StopMove(movable);
            return;
        }

        Vector3? targetPosition = movable.CurrentPath.GetCurrentTarget();
        if (!targetPosition.HasValue)
        {
            StopMove(movable);
            return;
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
                BeginRotate(target, direction);
        }
        else
        {
            if (target.IsRotating)
                StopRotate(target);
        }
    }
}
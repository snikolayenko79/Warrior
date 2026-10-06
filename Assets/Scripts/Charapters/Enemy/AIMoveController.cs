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
        if (MoveTarget == null || MoveTarget.IsDead)
            return;

        if (MoveTarget.CurrentPath == null)
        {
            StopMove();
            return;
        }

        Vector3? targetPosition = MoveTarget.CurrentPath.GetCurrentTarget();
        if (!targetPosition.HasValue)
        {
            StopMove();
            return;
        }
        
        if (!MoveTarget.IsMoving)
            BeginMove();
        
        if (Vector3.Distance(MoveTarget.Position, targetPosition.Value) <= 0.5f)
        {
            // Дошли до точки на маршруте
            MoveTarget.CurrentPath.NextTarget();
        }
        
        // Вращение в направлении точки на маршруте
        RotateTowards(MoveTarget, targetPosition.Value);
        
        base.Update();
    }

    private void RotateTowards(IMovable target, Vector3 targetPos)
    {
        Vector3 directionToPlayer = targetPos - MoveTarget.Position;
        directionToPlayer.y = 0;

        float forwardDot = Vector3.Dot(MoveTarget.Orientation, directionToPlayer.normalized);
        float sideDot = Vector3.Dot(MoveTarget.Right, directionToPlayer.normalized);
        float turnInput = Mathf.Clamp(sideDot, -1f, 1f);
        float angle = Mathf.Acos(forwardDot) * Mathf.Rad2Deg;
        if (angle > 1)
        {
            //Debug.Log(angle);
            // Vector3 r = MoveTarget.Rotation;
            // r.y += angle * turnInput;
            // MoveTarget.Rotation = Vector3.Lerp(MoveTarget.Rotation, r, Time.deltaTime * MoveTarget.MaxRotationSpeed);
            float direction = Mathf.Sign(angle * turnInput);
            if (!MoveTarget.IsRotating || MoveTarget.RotationDirection != direction)
                BeginRotate(direction);
        }
        else
        {
            if (MoveTarget.IsRotating)
                StopRotate();
        }
    }
}
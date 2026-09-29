using UnityEngine;

public class AIMoveController : CharapterMoveController
{
    private IAttackable _attackable;
    
    public void SetAttackable(IAttackable attackable)
    {
        _attackable = attackable;
        MoveTarget.IsMoving = true;
        // if (_attackable != null)
        // {
        //     Vector3 directionToPlayer = _attackable.Position - MoveTarget.Position;
        //     directionToPlayer.y = 0;
        //
        //     float sideDot = Vector3.Dot(MoveTarget.MyTransform.right, directionToPlayer.normalized);
        //     // float turnInput = Mathf.Clamp(sideDot, -1f, 1f);
        //     //
        //     // Vector3 currentForward = MoveTarget.MyTransform.up;
        //     // float forwardDot = Vector3.Dot(currentForward, directionToPlayer.normalized);
        //     //
        //     // // Если forwardDot меньше 0, значит игрок находится за спиной (угол больше 90 градусов)
        //     // if (forwardDot < 0)
        //     // {
        //     //     // Если игрок ровно позади (sideDot близко к 0), ИИ может заклинить. 
        //     //     // Принудительно выкручиваем руль вправо (1f), чтобы начать разворот
        //     //     if (Mathf.Abs(turnInput) < 0.05f)
        //     //     {
        //     //         turnInput = 1f; 
        //     //     }
        //     // }
        //     float angle = Mathf.Acos(sideDot) * Mathf.Rad2Deg;
        //     if (angle > 10)
        //     {
        //         //Debug.Log(angle);
        //         Vector3 r = MoveTarget.Rotation;
        //         r.y += angle;
        //         MoveTarget.Rotation = r; //Vector3.Lerp(MoveTarget.Rotation, r, Time.deltaTime);
        //     }
        // }
    }
    
    protected new void Update()
    {
        if (_attackable != null)
        {
            Vector3 directionToPlayer = _attackable.Position - MoveTarget.Position;
            directionToPlayer.y = 0;

            float forwardDot = Vector3.Dot(MoveTarget.Orientation, directionToPlayer.normalized);
            float sideDot = Vector3.Dot(MoveTarget.Right, directionToPlayer.normalized);
            float turnInput = Mathf.Clamp(sideDot, -1f, 1f);
            //
            // Vector3 currentForward = MoveTarget.MyTransform.up;
            // float forwardDot = Vector3.Dot(currentForward, directionToPlayer.normalized);
            //
            // // Если forwardDot меньше 0, значит игрок находится за спиной (угол больше 90 градусов)
            // if (forwardDot < 0)
            // {
            //     // Если игрок ровно позади (sideDot близко к 0), ИИ может заклинить. 
            //     // Принудительно выкручиваем руль вправо (1f), чтобы начать разворот
            //     if (Mathf.Abs(turnInput) < 0.05f)
            //     {
            //         turnInput = 1f; 
            //     }
            // }
            float angle = Mathf.Acos(forwardDot) * Mathf.Rad2Deg;
            if (angle > 1)
            {
                //Debug.Log(angle);
                Vector3 r = MoveTarget.Rotation;
                r.y += angle * turnInput;
                MoveTarget.Rotation = Vector3.Lerp(MoveTarget.Rotation, r, Time.deltaTime * MoveTarget.RotationSpeed);
            }
        }
        base.Update();
    }
}
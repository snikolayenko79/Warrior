using UnityEngine;

public interface IMoveController
{
    void BeginMove(IMovable movable);
    void StopMove(IMovable movable);
    void Move(IMovable movable);

    void BeginRotate(IMovable movable, float direction);
    void StopRotate(IMovable movable);
    void Rotate(IMovable movable);

    void UpdateMovement(IMovable movable);
}

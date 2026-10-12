using UnityEngine;

public interface ICharacterMoveController
{
    void RegisterEntity(IMovable movable);
    void UnregisterEntity(IMovable movable);

    void BeginMove(IMovable movable);
    void StopMove(IMovable movable);
    void Move(IMovable movable);

    void BeginRotate(IMovable movable, float direction);
    void StopRotate(IMovable movable);
    void Rotate(IMovable movable);

    void UpdateMovement(IMovable movable);
}

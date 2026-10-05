using UnityEngine;

public interface IMovable
{
    float MoveSpeed { get; set; }
    float MaxMoveSpeed { get; }
    Vector3 Orientation { get; }
    Vector3 Right { get; }
    bool IsMoving { get; set; }
    Vector3 Position { get; set; }
    Vector3 Rotation { get; set; }
    float RotationSpeed { get; set; }
    float MaxRotationSpeed { get; }
    float RotationDirection { get; set; }
    bool IsRotating { get; }
    bool IsDead { get; }
}
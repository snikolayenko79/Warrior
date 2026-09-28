using UnityEngine;

public interface IMovable
{
    float MoveSpeed { get; }
    Vector3 Orientation { get; }
    bool IsMoving { get; set; }
    Vector3 Position { get; set; }
}
using UnityEngine;

public interface IRotable
{
    float RotationSpeed { get; }
    float RotationDirection { get; set; }
    bool IsRotating { get; }
    Vector3 Rotation { get; set; }
}
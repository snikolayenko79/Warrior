using UnityEngine;

public interface IAttackable
{
    Vector3 Position { get; }
    bool IsDead { get; }
    SideType Side { get; }
}
using UnityEngine;
using System.Collections.Generic;

public interface IAttacker
{
    Vector3 Position { get; set; }
    WeaponData SelectedWeapon { get; set; }
    Transform WeaponPoint { get; }
    GameObject CurrentWeaponObject { get; set; }
    bool IsAttacking { get; set; }
    bool IsAttackStarted { get; set; }
    void Attack();
    bool IsDead { get; }
    int Experience { get; set; } // 0-100
    float LockAttackCounter { get; set; }
}

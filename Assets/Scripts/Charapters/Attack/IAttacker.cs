using UnityEngine;
using System.Collections.Generic;

public interface IAttacker
{
    Vector3 Position { get; set; }
    List<WeaponData> AllWeapons { get; }
    List<WeaponData> AvailableWeapons { get; set; }
    WeaponData CurrentWeapon { get; set; }
    Transform WeaponPoint { get; }
    GameObject CurrentWeaponObject { get; set; }
    bool IsAttacking { get; set; }
    bool IsAttackStarted { get; set; }
    void Attack();
    bool IsDead { get; }
}

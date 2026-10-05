using UnityEngine;
using System.Collections.Generic;

public interface IAttacker
{
    List<WeaponData> AllWeapons { get; }
    List<WeaponData> AvailableWeapons { get; set; }
    WeaponData CurrentWeapon { get; set; }
    Transform WeaponPoint { get; }
    GameObject CurrentWeaponObject { get; set; }
    void Attack();
}

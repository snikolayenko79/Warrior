using UnityEngine;

public interface IAttackController
{
    void UpdateAttack(IAttacker attacker);
    void AddWeapon(IAttacker attacker, WeaponData weapon);
    void SelectWeapon(IAttacker attacker, WeaponData weapon);
}

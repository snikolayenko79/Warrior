using UnityEngine;

public interface IAttackController
{
    void SelectWeapon(IAttacker attacker, WeaponData weapon);
    
    void UpdateAttack(IAttacker attacker);
}

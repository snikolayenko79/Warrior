using UnityEngine;
using System.Collections.Generic;

public interface IAttackController
{
    void RegisterAttacker(IAttacker attacker);
    void UnregisterAttacker(IAttacker attacker);
    
    void RegisterAttackable(IAttackable attackable);
    void UnregisterAttackable(IAttackable attackable);
    
    void SelectWeapon(IAttacker attacker, WeaponData weapon);
    
    void UpdateAttack(IAttacker attacker);
    
    bool CanAttack(IAttacker attacker);
    bool CanAttack(IAttacker attacker, IAttackable attackable);
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladedWeapon : WeaponBehaviour
{
    [Tooltip("Задержка перед применением. Чтобы синхронизировать с анимацией удара.")]
    public float delay = 0;
    
    private bool _isEnded = false;
    
    public override void Execute(IAttacker attacker, OnWeaponReadyToDamage onWeaponReadyToDamage)
    {
        StartCoroutine(ExecuteCoroutine(attacker, onWeaponReadyToDamage));
    }

    public override bool IsEnded => _isEnded;

    private IEnumerator ExecuteCoroutine(IAttacker caster, OnWeaponReadyToDamage onWeaponReadyToDamage)
    {
        Collider weaponCollider = caster.CurrentWeaponObject.GetComponent<Collider>();
        
        yield   return new WaitForSeconds(delay);
        
        onWeaponReadyToDamage(weaponCollider);
        _isEnded = true;
    }
}

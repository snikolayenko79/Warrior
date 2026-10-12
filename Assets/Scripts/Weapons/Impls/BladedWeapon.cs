using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladedWeapon : WeaponBehaviour
{
    [Tooltip("Задержка перед применением. Чтобы синхронизировать с анимацией удара.")]
    public float delay = 0;
    
    public override void Execute(IAttacker attacker, OnWeaponReadyToDamage onWeaponReadyToDamage)
    {
        StartCoroutine(ExecuteCoroutine(attacker, onWeaponReadyToDamage));
    }

    private IEnumerator ExecuteCoroutine(IAttacker caster, OnWeaponReadyToDamage onWeaponReadyToDamage)
    {
        Collider weaponCollider = caster.CurrentWeaponObject.GetComponent<Collider>();
        
        yield return new WaitForSeconds(delay);
        
        onWeaponReadyToDamage(weaponCollider);
    }
}

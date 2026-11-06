using UnityEngine;
using System.Collections.Generic;

public class AttackController : MonoBehaviour, IAttackController
{
    [Tooltip("Леер врагов (противоположной стороны).")]
    public LayerMask playerLayer;
    public LayerMask enemyLayer;
    
    public float attackDistance = 0.3f;
    
    private List<IAttacker> _attackers = new List<IAttacker>();
    private List<IAttackable> _attackables = new List<IAttackable>();

    public void RegisterAttacker(IAttacker attacker)
    {
        _attackers.Add(attacker);
    }

    public void UnregisterAttacker(IAttacker attacker)
    {
        _attackers.Remove(attacker);
    }

    public void RegisterAttackable(IAttackable attackable)
    {
        _attackables.Add(attackable);
    }

    public void UnregisterAttackable(IAttackable attackable)
    {
        _attackables.Remove(attackable);
    }

    public void UpdateAttack(IAttacker attacker)
    {
        if (attacker is { IsAttacking: true})
        {
            if (CanAttack(attacker))
                DoAttack(attacker);
            
            attacker.IsAttacking = false;
        }
    }

    public bool CanAttack(IAttacker attacker)
    {
        if (attacker.SelectedWeapon.IsUnityNull())
            return  false;
        
        bool bCanAttack = false;
        
        foreach (var attackable in _attackables)
        {
            if (CanAttack(attacker, attackable))
            {
                bCanAttack = true;
                break;
            }
        }
        
        return bCanAttack;
    }
    
    public bool CanAttack(IAttacker attacker, IAttackable attackable)
    {
        if (attackable.IsDead)
            return false;
            
        if (attacker.Side == SideType.Player && attackable.Side != SideType.Enemy)
            return false;
            
        if (attacker.Side == SideType.Enemy && attackable.Side != SideType.Player)
            return false;

        return Vector3.Distance(attacker.Position, attackable.Position) <= attackDistance;
    }

    protected void DoAttack(IAttacker attacker)
    {
        if (attacker is { IsAttackStarted: true })
        {
            return;
        }

        if (attacker.SelectedWeapon == null)
            return;
        
        attacker.IsAttackStarted = true;
        attacker.Attack();

        WeaponData selectedWeapon = attacker.SelectedWeapon;
        WeaponBehaviour instantiatedWeaponBehaviour = Instantiate(selectedWeapon.weaponBehaviourPrefab, attacker.WeaponPoint.position, selectedWeapon.weaponBehaviourPrefab.transform.rotation);
        instantiatedWeaponBehaviour.transform.SetParent(attacker.WeaponPoint, true);
        instantiatedWeaponBehaviour.Execute(attacker, (Collider damageZone) =>
        {
            // 1. Получаем размеры и координаты зоны
            Vector3 center = damageZone.bounds.center;
            Vector3 halfExtents = damageZone.bounds.extents;
            Quaternion orientation = damageZone.transform.rotation;

            // 2. Находим все коллайдеры врагов в зоне
            Collider[] hitColliders = Physics.OverlapBox(center, halfExtents, orientation, attacker is Enemy ? playerLayer : enemyLayer);

            // 3. Перебираем им и наносим урон через интерфейс
            foreach (Collider enemyCollider in hitColliders)
            {
                //Debug.Log(enemyCollider.gameObject.name, enemyCollider.gameObject);
                // TryGetComponent ищет интерфейс IDamageable на объекте без лишних аллокаций
                if (enemyCollider.TryGetComponent<IDamageable>(out var damageable))
                {
                    if (!damageable.IsDead)
                    {
                        damageable.TakeDamage(selectedWeapon.weaponDamage);
                        if (damageable.Health <= 0)
                            damageable.Dead();
                    }
                }
            }
            
            Destroy(instantiatedWeaponBehaviour.gameObject);
            instantiatedWeaponBehaviour = null;
            
            if (attacker != null)
                attacker.IsAttackStarted = false;
        });
    }
    
    public void SelectWeapon(IAttacker attacker, WeaponData weapon)
    {
        attacker.SelectedWeapon = weapon;
        
        if (attacker.CurrentWeaponObject)
            Destroy(attacker.CurrentWeaponObject);
        
        attacker.CurrentWeaponObject = Instantiate(attacker.SelectedWeapon.weaponPrefab, attacker.WeaponPoint);
    }
}
using UnityEngine;

public class AttackController : MonoBehaviour, IAttackController
{
    [Tooltip("Леер врагов (противоположной стороны).")]
    public LayerMask playerLayer;
    public LayerMask enemyLayer;

    public void UpdateAttack(IAttacker attacker)
    {
        if (attacker is { IsAttacking: true})
        {
            DoAttack(attacker);
            attacker.IsAttacking = false;
        }
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
using UnityEngine;

public class AttackController : MonoBehaviour
{
    [Tooltip("Леер врагов (противоположной стороны).")]
    public LayerMask enemyLayer;

    private WeaponBehaviour instantiatedWeaponBehaviour = null;
    
    protected IAttacker Attacker;

    public void SetAttacker(IAttacker attacker)
    {
        Attacker = attacker;
        
        // TODO. Временно. Удалить. Выбор заклинания.
        Attacker.AvailableWeapons.AddRange(Attacker.AllWeapons);
        SelectWeapon(Attacker.AvailableWeapons[0]);
    }

    protected void DoAttack()
    {
        if (Attacker == null || Attacker.CurrentWeapon == null)
            return;

        if (instantiatedWeaponBehaviour is { IsEnded : false})
            return;
        
        if (instantiatedWeaponBehaviour)
        {
            if (!instantiatedWeaponBehaviour.IsEnded)
                return;
            
            Destroy(instantiatedWeaponBehaviour.gameObject);
            instantiatedWeaponBehaviour = null;
        }
        
        Attacker.Attack();

        WeaponData selectedWeapon = Attacker.CurrentWeapon;
        instantiatedWeaponBehaviour = Instantiate(selectedWeapon.weaponBehaviourPrefab, Attacker.WeaponPoint.position, selectedWeapon.weaponBehaviourPrefab.transform.rotation);
        instantiatedWeaponBehaviour.transform.SetParent(Attacker.WeaponPoint, true);
        instantiatedWeaponBehaviour.Execute(Attacker, (Collider damageZone) =>
        {
            // 1. Получаем размеры и координаты зоны
            Vector3 center = damageZone.bounds.center;
            Vector3 halfExtents = damageZone.bounds.extents;
            Quaternion orientation = damageZone.transform.rotation;

            // 2. Находим все коллайдеры врагов в зоне
            Collider[] hitColliders = Physics.OverlapBox(center, halfExtents, orientation, enemyLayer);

            // 3. Перебираем им и наносим урон через интерфейс
            foreach (Collider enemyCollider in hitColliders)
            {
                //Debug.Log(enemyCollider.gameObject.name, enemyCollider.gameObject);
                // TryGetComponent ищет интерфейс IDamageable на объекте без лишних аллокаций
                if (enemyCollider.TryGetComponent<IDamageable>(out var damageable))
                {
                    if (!damageable.IsDead)
                        damageable.TakeDamage(selectedWeapon.weaponDamage);
                }
            }
            
            Destroy(instantiatedWeaponBehaviour.gameObject);
            instantiatedWeaponBehaviour = null;
        });
    }

    private void SelectWeapon(WeaponData weapon)
    {
        Attacker.CurrentWeapon = weapon;
        
        if (Attacker.CurrentWeaponObject)
            Destroy(Attacker.CurrentWeaponObject);
        
        Attacker.CurrentWeaponObject = Instantiate(Attacker.CurrentWeapon.weaponPrefab, Attacker.WeaponPoint);
    }
}
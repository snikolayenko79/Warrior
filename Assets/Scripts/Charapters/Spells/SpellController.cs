using UnityEngine;

public class SpellController : MonoBehaviour, ISpellController
{   
    [Tooltip("Леер врагов (противоположной стороны).")]
    public LayerMask enemyLayer;

    public void UpdateSpellCast(ISpellCaster caster)
    {
        if (caster is { IsSpellCast: true})
        {
            DoSpellCast(caster);
            caster.IsSpellCast = false;
        }
    }

    protected void DoSpellCast(ISpellCaster caster)
    {
        if (caster is { IsSpellCastStarted: true})
            return;
        
        if (caster.CurrentSpell == null)
        {
            caster.AvailableSpells.AddRange(caster.AllSpells);
            caster.CurrentSpell = caster.AvailableSpells[0];
        }
        
        if (caster.CurrentSpell == null)
            return;
        
        caster.IsSpellCastStarted = true;
        caster.Spell();

        SpellData selectedSpell = caster.CurrentSpell;
        SpellBehaviour instantiatedSpell = Instantiate(selectedSpell.spellBehaviourPrefab, this.transform);
        instantiatedSpell.Execute(caster, (Collider damageZone) =>
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
                        damageable.TakeDamage(selectedSpell.spellDamage);
                }
            }
            
            if (caster != null)
                caster.IsSpellCastStarted = false;
        });
    }
}
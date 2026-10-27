using UnityEngine;

public class SpellController : MonoBehaviour, ISpellController
{   
    [Tooltip("Леер врагов (противоположной стороны).")]
    public LayerMask enemyLayer;
    
    public void UpdateSpellCast(ISpellCaster caster)
    {
        if (caster is { IsSpellCast: true})
        {
            DoSpellCast(caster, caster.SelectedSpell);
            caster.IsSpellCast = false;
            caster.SelectedSpell = null;
        }
    }

    protected void DoSpellCast(ISpellCaster caster, SpellData spell)
    {
        if (caster is { IsSpellCastStarted: true})
            return;
        
        caster.IsSpellCastStarted = true;
        caster.Spell();
        
        SpellBehaviour instantiatedSpell = Instantiate(spell.spellBehaviourPrefab, this.transform);
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
                        damageable.TakeDamage(spell.spellDamage);
                }
            }
            
            if (caster != null)
                caster.IsSpellCastStarted = false;
        });
    }
    
    public void AddSpell(ISpellCaster caster, SpellData spell)
    {
        if (!caster.AvailableSpells.Contains(spell))
        {
            caster.AvailableSpells.Add(spell);
        }
    }
}
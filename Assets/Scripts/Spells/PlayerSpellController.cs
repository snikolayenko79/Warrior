using UnityEngine;

public class PlayerSpellController : SpellController
{
    public LayerMask enemyLayer;
    
    private IInputListener _inputListener;

    private bool _isInitialized;
    
    public void SetInputListener(IInputListener inputListener)
    {
        _inputListener  = inputListener;
        _inputListener.OnInputAction += OnInputAction;
    }
    
    protected virtual void OnEnable()
    {
        if (_isInitialized && _inputListener != null)
        {
            _inputListener.OnInputAction += OnInputAction;
        }
    }
    
    protected virtual void OnDisable()
    {
        if (_isInitialized && _inputListener != null)
        {
            _inputListener.OnInputAction -= OnInputAction;
        }
    }
    
    private void OnInputAction(string logicalActionName, InputContext context)
    {
        if (logicalActionName == "Spell" && context.FloatValue == 0)
        {
            DoSpellCast();
        }
    }
    
    private void DoSpellCast()
    {
        if (SpellCaster == null || SpellCaster.CurrentSpell == null)
            return;
        
        SpellCaster.Spell();

        SpellData selectedSpell = SpellCaster.CurrentSpell;
        SpellBehaviour instantiatedSpell = Instantiate(selectedSpell.spellBehaviourPrefab, this.transform);
        instantiatedSpell.Execute(SpellCaster, (Collider damageZone) =>
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
                Debug.Log(enemyCollider.gameObject.name, enemyCollider.gameObject);
                // TryGetComponent ищет интерфейс IDamageable на объекте без лишних аллокаций
                if (enemyCollider.TryGetComponent<IDamageable>(out var damageable))
                {
                    if (!damageable.IsDead)
                        damageable.TakeDamage(selectedSpell.spellDamage);
                }
            }
        });
    }
}

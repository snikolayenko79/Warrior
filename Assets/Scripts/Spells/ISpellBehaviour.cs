using UnityEngine;

public interface ISpellBehaviour
{
    void Execute(ISpellCaster caster, OnSpellReadyToDamage  onSpellReadyToDamage);
}

public delegate void OnSpellReadyToDamage(Collider damageZone);

public abstract class SpellBehaviour : MonoBehaviour, ISpellBehaviour
{
    public abstract void Execute(ISpellCaster caster, OnSpellReadyToDamage  onSpellReadyToDamage);
}
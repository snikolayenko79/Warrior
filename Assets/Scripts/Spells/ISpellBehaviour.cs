using UnityEngine;

public interface ISpellBehaviour
{
    void Execute(ISpellCaster caster);
}

public abstract class SpellBehaviour : MonoBehaviour, ISpellBehaviour
{
    public abstract void Execute(ISpellCaster caster);
}
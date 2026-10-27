using UnityEngine;

public interface ISpellController
{
    void UpdateSpellCast(ISpellCaster caster);
    void AddSpell(ISpellCaster caster, SpellData spell);
}
using UnityEngine;
using System.Collections.Generic;

public interface ISpellCaster
{
    List<SpellData> AvailableSpells { get; set; }
    Transform SpellSpawnPoint { get; }
    void Spell(SpellData spell = null);
    bool IsSpellCast { get; set; }
    SpellData SelectedSpell { get; set; }
    bool IsSpellCastStarted { get; set; }
    bool IsDead { get; }
}
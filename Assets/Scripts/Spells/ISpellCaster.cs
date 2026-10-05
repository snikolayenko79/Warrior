using UnityEngine;
using System.Collections.Generic;

public interface ISpellCaster
{
    List<SpellData> AllSpells { get; }
    List<SpellData> AvailableSpells { get; set; }
    SpellData CurrentSpell { get; set; }
    Transform SpellSpawnPoint { get; }
    void Spell(SpellData spell = null);
}
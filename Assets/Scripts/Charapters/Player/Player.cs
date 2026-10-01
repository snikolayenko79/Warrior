using System;
using UnityEngine;
using System.Collections.Generic;

public class Player : Charapter, IAttackable, ISpellCaster
{
    [SerializeField] private List<SpellData> allSpells = new List<SpellData>();
    private List<SpellData> _availableSpells = new List<SpellData>();
    private SpellData _currentSpell;
    [SerializeField] Transform spellSpawnPoint;
    
    public List<SpellData> AllSpells => allSpells;

    public List<SpellData> AvailableSpells
    {
        get => _availableSpells;
        set  => _availableSpells = value;
    }

    public SpellData CurrentSpell
    {
        get => _currentSpell;
        set  => _currentSpell = value;
    }

    public Transform SpellSpawnPoint => spellSpawnPoint;

    public void Spell(SpellData spell = null)
    {
        if (MyAnimator != null)
            MyAnimator.SetTrigger("Spell");
    }

    private void Start()
    {
        // TODO. Временно. Удалить.
        _availableSpells.AddRange(AllSpells);
        CurrentSpell = _availableSpells[0];
    }
}
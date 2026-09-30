using UnityEngine;
using System.Collections.Generic;

public class Player : Charapter, IAttackable, ISpellCaster
{
    [SerializeField] private List<SpellData> allSpells = new List<SpellData>();
    private List<SpellData> _availableSpells = new List<SpellData>();
    private SpellData _currentSpell;
    [SerializeField] Vector3 spawnPoint;
    
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

    public Vector3 SpawnPoint => spawnPoint;

    public void Spell(SpellData spell = null)
    {
        if (MyAnimator != null)
            MyAnimator.SetTrigger("Spell");
    }
}
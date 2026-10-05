using System;
using UnityEngine;
using System.Collections.Generic;

public class Player : Charapter, IAttacker, ISpellCaster, IAttackable
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
    
    [SerializeField] private List<WeaponData> allWeapons = new List<WeaponData>();
    private List<WeaponData> _availableWeapons = new List<WeaponData>();
    private WeaponData _currentWeapon;
    [SerializeField] Transform weaponPoint;
    private GameObject _currentWeaponObject;

    public List<WeaponData> AllWeapons => allWeapons;

    public List<WeaponData> AvailableWeapons
    {
        get => _availableWeapons;
        set  => _availableWeapons = value;
    }
    
    public WeaponData CurrentWeapon
    {
        get => _currentWeapon;
        set  => _currentWeapon = value;
    }
    
    public Transform WeaponPoint => weaponPoint;
    public GameObject CurrentWeaponObject
    {
        get => _currentWeaponObject;
        set  => _currentWeaponObject = value;
    }
    
    public void Attack()
    {
        if (MyAnimator != null)
            MyAnimator.SetTrigger("Attack");
    }
}
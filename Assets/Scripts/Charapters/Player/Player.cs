using System;
using UnityEngine;
using System.Collections.Generic;

public class Player : ActorCharapter, IAttacker, ISpellCaster, IAttackable, ICustomer
{
    private List<SpellData> _availableSpells = new List<SpellData>();
    [SerializeField] Transform spellSpawnPoint;
    
    public List<SpellData> AvailableSpells
    {
        get => _availableSpells;
        set  => _availableSpells = value;
    }

    public Transform SpellSpawnPoint => spellSpawnPoint;

    public void Spell(SpellData spell = null)
    {
        if (MyAnimator != null)
            MyAnimator.SetTrigger("Spell");
    }
    
    private bool _isSpellCast = false;
    public bool IsSpellCast
    {
        get => _isSpellCast;
        set => _isSpellCast = value;
    }

    private SpellData _selectedSpell = null;
    public SpellData SelectedSpell
    {
        get => _selectedSpell;
        set => _selectedSpell = value;
    }

    private bool _isSpellCastStarted  = false;
    public bool IsSpellCastStarted
    {
        get => _isSpellCastStarted;
        set => _isSpellCastStarted = value;
    }
    
    private List<WeaponData> _availableWeapons = new List<WeaponData>();
    private WeaponData _selectedWeapon;
    [SerializeField] Transform weaponPoint;
    private GameObject _currentWeaponObject;

    public List<WeaponData> AvailableWeapons
    {
        get => _availableWeapons;
        set  => _availableWeapons = value;
    }
    
    public WeaponData SelectedWeapon
    {
        get => _selectedWeapon;
        set  => _selectedWeapon = value;
    }
    
    public Transform WeaponPoint => weaponPoint;
    public GameObject CurrentWeaponObject
    {
        get => _currentWeaponObject;
        set  => _currentWeaponObject = value;
    }

    private bool _isAttacking  = false;
    public bool IsAttacking
    {
        get => _isAttacking;
        set => _isAttacking = value;
    }
    
    private bool _isAttackStarted  = false;
    public bool IsAttackStarted
    {
        get => _isAttackStarted;
        set => _isAttackStarted = value;
    }
    
    public void Attack()
    {
        if (MyAnimator != null)
            MyAnimator.SetTrigger("Attack");
    }
    
    [SerializeField] private int _experience = 10;
    
    public int Experience
    {
        get => _experience;
        set  => _experience = value;
        
    }
    
    private float _lockAttackCounter = 0;
    
    public float LockAttackCounter
    {
        get => _lockAttackCounter;
        set  => _lockAttackCounter = value;
        
    }

    private int _coins = 100;

    public int Coins
    {
        get => _coins;
        set => _coins = value;
    }
}
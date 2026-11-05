using System;
using UnityEngine;
using System.Collections.Generic;
using Zenject;

public class Player : ActorCharapter, IAttacker, ISpellCaster, IAttackable, ICustomer, IInventoryOwner, IDamageable
{
    private IPlayerData _playerData;
    private IInventory _inventory;
    
    protected new void Awake()
    {
        base.Awake();
        
        if (!TryGetComponent<IPlayerData>(out _playerData))
        {
            Debug.LogError($"[EnemyView] На объекте {name} отсутствует обязательный компонент данных IPlayerData!", this);
        }
    }
    
    public IInventory CurrentInventory
    {
        get => _inventory;
        set => _inventory = value;
    }
    
    [Inject]
    public void Construct(IInventory inventory)
    {
        _inventory = inventory;
        _inventory.Owner = this;
    }
    
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
    
    private WeaponData _selectedWeapon;
    [SerializeField] Transform weaponPoint;
    private GameObject _currentWeaponObject;
    
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
    
    public int Experience
    {
        get => _playerData.Experience;
        set  => _playerData.Experience = value;
        
    }
    
    private float _lockAttackCounter = 0;
    
    public float LockAttackCounter
    {
        get => _lockAttackCounter;
        set  => _lockAttackCounter = value;
        
    }

    public int Coins
    {
        get => _playerData.Coins;
        set => _playerData.Coins = value;
    }
    
    public override bool IsDead => _playerData.CurrentHealth <= 0;

    public float Health
    {
        get => _playerData.CurrentHealth;
        set => _playerData.CurrentHealth = value;
    }

    public void TakeDamage(float damageAmount)
    {
        Debug.Log(this.name + ": -" + damageAmount + " health points");
        
        _playerData.CurrentHealth -= damageAmount;
        
        if (MyAnimator)
            MyAnimator.SetTrigger("TakeDamage");

        if (Health <= 0)
            Dead();
    }
    
    public void Dead()
    {
        if (MyAnimator)
        {
            MyAnimator.SetBool("IsMoving", false);
            MyAnimator.SetTrigger("IsDead");
        }

        MyRigidbody.isKinematic = true;
        
        OnDead?.Invoke(this);
    }
}
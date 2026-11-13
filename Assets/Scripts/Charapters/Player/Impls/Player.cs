using System;
using UnityEngine;
using System.Collections.Generic;
using Zenject;

public class Player : AttackCharacter, ISpellCaster, ICustomer, IInventoryOwner
{
    [SerializeField]private PlayerData playerData;
    private IInventory _inventory;
    
    private PlayerData _currentPlayerData;
    public PlayerData CurrentPlayerData
    {
        get
        {
            if (_currentPlayerData == null)
                _currentPlayerData = CharacterData as PlayerData;
            
            return _currentPlayerData;
        }
    }
    
    [Inject]
    public void Construct(IInventory inventory)
    {
        _inventory = inventory;
        _inventory.Owner = this;
    }
    
    protected new void Awake()
    {
        base.Awake();
        
        if (!playerData)
            Debug.LogError($"[Player] На объекте {name} отсутствует обязательный компонент данных PlayerData!", this);
    }
    
    public override void InitializeData()
    {
        CharacterData = playerData;
        base.InitializeData();
    }
    
    public IInventory CurrentInventory
    {
        get => _inventory;
        set => _inventory = value;
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
    
    public int Coins
    {
        get => playerData.Coins;
        set => playerData.Coins = value;
    }
}
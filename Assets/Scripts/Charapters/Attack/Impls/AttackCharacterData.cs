using UnityEngine;
using System;

public class AttackCharacterData : CharacterData, IAttackCharacterData
{
    [Header("Init data")]
    [SerializeField]protected float maxHealthInit;
    [SerializeField]protected int experienceInit;
    
    protected float _currentHealth;
    protected float _currentMaxHealth;
    protected int _currentExperience;
    
    public float Health
    {
        get => _currentHealth;
        set
        {
            _currentHealth = Mathf.Clamp(value, 0, _currentMaxHealth);
            RaiseDataChanged();
        }
    }

    public float MaxHealth
    {
        get => _currentMaxHealth;
        set
        {
            _currentMaxHealth = value;
            RaiseDataChanged();
        }
    }

    public int Experience
    {
        get => _currentExperience;
        set
        {
            _currentExperience = value;
            RaiseDataChanged();
        }
    }

    public override void Initialize()
    {
        _currentMaxHealth = maxHealthInit;
        _currentHealth = _currentMaxHealth;
        _currentExperience = experienceInit;
        
        RaiseDataInitialized();
    }
}
using UnityEngine;

public class AttackCharacterData : CharacterData, IAttackCharacterData
{
    [Header("Init data")]
    [SerializeField]protected float maxHealthInit;
    [SerializeField]protected int experienceInit;
    
    private float _health;
    private float _currentMaxHealth;
    private int _currentExperience;
    
    public float Health
    {
        get => _health;
        set => _health = Mathf.Clamp(value, 0, _currentMaxHealth);
    }
    
    public float MaxHealth 
    {
        get => _currentMaxHealth;
        set => _currentMaxHealth = value;
    }

    public int Experience
    {
        get => _currentExperience;
        set => _currentExperience = value;
    }
    
    public override void Initialize()
    {
        _currentMaxHealth = maxHealthInit;
        _health = _currentMaxHealth;
        _currentExperience = experienceInit;
    }
}
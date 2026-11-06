using System;
using UnityEngine;

public class EnemyData : MonoBehaviour, IEnemyData
{
    [SerializeField]private float maxHealth;
    [SerializeField]private int experience;
    
    private float _currentHealth;
    
    public float CurrentHealth
    {
        get => _currentHealth;
        set => _currentHealth = Mathf.Clamp(value, 0, maxHealth);
    }
    
    public float MaxHealth => maxHealth;
    
    public int Experience
    {
        get => experience;
        set => experience = value;
    }
    
    public void Initialize()
    {
        _currentHealth = maxHealth;
    }
}
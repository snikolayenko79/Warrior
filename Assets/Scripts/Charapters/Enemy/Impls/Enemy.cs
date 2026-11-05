using UnityEngine;
using System.Collections.Generic;

public class Enemy : ActorCharapter, IAttacker, IAttackable, IDamageable
{
    private IEnemyData _enemyData;

    protected new void Awake()
    {
        base.Awake();
        
        if (!TryGetComponent<IEnemyData>(out _enemyData))
        {
            Debug.LogError($"[EnemyView] На объекте {name} отсутствует обязательный компонент данных IEnemyData!", this);
        }
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
    
    public WeaponData SelectedWeapon
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
        get => _enemyData.Experience;
        set  => _enemyData.Experience = value;
        
    }
    
    private float _lockAttackCounter = 0;
    
    public float LockAttackCounter
    {
        get => _lockAttackCounter;
        set  => _lockAttackCounter = value;
        
    }
    
    public override bool IsDead => _enemyData.CurrentHealth <= 0;

    public float Health
    {
        get => _enemyData.CurrentHealth;
        set => _enemyData.CurrentHealth = value;
    }

    public void TakeDamage(float damageAmount)
    {
        Debug.Log(this.name + ": -" + damageAmount + " health points");
        
        _enemyData.CurrentHealth -= damageAmount;
        
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
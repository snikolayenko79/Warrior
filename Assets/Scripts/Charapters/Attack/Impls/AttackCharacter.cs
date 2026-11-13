using UnityEngine;
using Zenject;

public class AttackCharacter : ActorCharacter, IAttacker, IAttackable, IDamageable
{
    public AudioSource attackSound;
    public AudioSource damageSound;
    public AudioSource deadSound;
    
    private IAttackController _attackController;

    private IAttackCharacterData _currentAttackCharacterData;

    public IAttackCharacterData CurrentAttackCharacterData
    {
        get
        {
            if (_currentAttackCharacterData == null)
                _currentAttackCharacterData = CharacterData as IAttackCharacterData;
            
            return _currentAttackCharacterData;
        }
    }

    [Inject]
    public void Construct(IAttackController attackController)
    {
        _attackController = attackController;
    }
    
    protected new void OnEnable()
    {
        base.OnEnable();
        
        _attackController.RegisterAttacker(this);
        _attackController.RegisterAttackable(this);
    }
    
    protected new void OnDisable()
    {
        base.OnDisable();
        
        _attackController.UnregisterAttacker(this);
        _attackController.UnregisterAttackable(this);
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
        
        if (attackSound)
            attackSound.Play();
    }
    
    [SerializeField] private SideType side;

    public SideType Side => side;

    public int Experience
    {
        get => CurrentAttackCharacterData.Experience;
        set => CurrentAttackCharacterData.Experience = value;
    }
    
    private float _lockAttackCounter = 0;
    
    public float LockAttackCounter
    {
        get => _lockAttackCounter;
        set  => _lockAttackCounter = value;
    }
    
    public override bool IsDead => CurrentAttackCharacterData.Health <= 0;

    public float Health
    {
        get => CurrentAttackCharacterData.Health;
        set => CurrentAttackCharacterData.Health = value;
    }
    
    public float MaxHealth
    {
        get => CurrentAttackCharacterData.MaxHealth;
        set => CurrentAttackCharacterData.MaxHealth = value;
    }

    public void TakeDamage(float damageAmount)
    {
        Debug.Log(this.name + ": -" + damageAmount + " health points");
        
        CurrentAttackCharacterData.Health -= damageAmount;
        
        if (MyAnimator)
            MyAnimator.SetTrigger("TakeDamage");

        if (damageSound)
            damageSound.Play();
        
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
        
        if (deadSound)
            deadSound.Play();

        MyRigidbody.isKinematic = true;
        
        OnDead?.Invoke(this);
    }
}
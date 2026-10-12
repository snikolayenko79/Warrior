using UnityEngine;
using System;

public class Character : MonoBehaviour, IMovable, IDamageable
{
    public Animator MyAnimator;

    [SerializeField] private float MyMoveSpeed = 0.5f;

    private float _currentMoveSpeed = 0;
    
    public float MoveSpeed
    {
        get => _currentMoveSpeed;
        set  => _currentMoveSpeed = value;
    }

    public float MaxMoveSpeed => MyMoveSpeed;

    public Vector3 Orientation
    {
        // TODO. настраивать в редакторе.
        get => -this.transform.up;
    }
    
    public Vector3 Right
    {
        // TODO. настраивать в редакторе.
        get => this.transform.right;
    }
    
    private bool isMoving = false;

    public bool IsMoving
    {
        get => isMoving;
        set
        {
            isMoving = value;
            OnMoveChange();
        }
    }

    public Vector3 Position
    {
        get => transform.position;
        set => transform.position = value;
    }
    
    private INavigationPath path;

    public INavigationPath CurrentPath
    {
        get => path;
        set => path = value;
    }
    
    public Vector3 Rotation
    {
        get => transform.eulerAngles;
        set  => transform.rotation = Quaternion.Euler(value);
    }
    
    [SerializeField] private float MyRotateSpeed = 100;
    
    private float _currentRotateSpeed = 0;

    public float RotationSpeed
    {
        get => _currentRotateSpeed;
        set  => _currentRotateSpeed = value;
    }

    public float MaxRotationSpeed => MyRotateSpeed;

    private float _rotateDirection = 0;

    public float RotationDirection
    {
        get => _rotateDirection;
        set => _rotateDirection = value;
    }

    public bool IsRotating => RotationDirection != 0;
    
    public Transform MyTransform => this.transform;

    protected float health = 100;

    protected void Awake()
    {
        if (!MyAnimator)
            MyAnimator = this.GetComponent<Animator>();
        
        if (CurrentPath == null)
            CurrentPath = GetComponent<INavigationPath>();
    }
    
    void OnMoveChange()
    {
        if (MyAnimator)
            MyAnimator.SetBool("IsMoving", IsMoving);
    }
    
    public bool IsDead => health <= 0;

    public float Health => health;
    
    public void TakeDamage(float damageAmount)
    {
        Debug.Log(this.name + ": -" + damageAmount + " health points");
        
        health -= damageAmount;
        health = Mathf.Clamp(Health, 0, 100);
        
        if (MyAnimator)
            MyAnimator.SetTrigger("TakeDamage");

        if (Health <= 0)
            Dead();
    }

    public Action<Character> OnDead;
    
    public void Dead()
    {
        if (MyAnimator)
        {
            MyAnimator.SetBool("IsMoving", false);
            MyAnimator.SetTrigger("IsDead");
        }
        
        OnDead?.Invoke(this);
    }
}
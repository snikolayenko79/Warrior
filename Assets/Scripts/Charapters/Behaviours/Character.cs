using UnityEngine;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class Character : MonoBehaviour, IMovable, ICharapterState
{
    public Animator MyAnimator;

    private List<IBehaviour> _stateBehaviours = new List<IBehaviour>();

    public List<IBehaviour> StateBehaviours
    {
        get => _stateBehaviours;
        set => _stateBehaviours = value;
    }
    
    public Rigidbody MyRigidbody;
    
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
    
    private bool isMovingLock = false;

    public bool IsMovingLock
    {
        get => isMovingLock;
        set => isMovingLock = value;
    }

    public Vector3 Position
    {
        get => MyRigidbody.position;
        set
        {
            MyRigidbody.MovePosition(value);
        }
    }
    
    private INavigationPath path;

    public INavigationPath CurrentPath
    {
        get => path;
        set => path = value;
    }
    
    public Vector3 Rotation
    {
        get => MyRigidbody.rotation.eulerAngles;
        set  => MyRigidbody.MoveRotation(Quaternion.Euler(value));
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

    protected void Awake()
    {
        if (!MyAnimator)
            MyAnimator = this.GetComponent<Animator>();
        
        if (!MyRigidbody)
            MyRigidbody = this.GetComponent<Rigidbody>();
        
        if (CurrentPath == null)
            CurrentPath = GetComponent<INavigationPath>();
    }

    protected void OnEnable() { }
    
    protected void OnDisable() { }

    void OnMoveChange()
    {
        if (MyAnimator)
            MyAnimator.SetBool("IsMoving", IsMoving);
    }
    
    public virtual bool IsDead => false;
    
    public Action<Character> OnDead;
}
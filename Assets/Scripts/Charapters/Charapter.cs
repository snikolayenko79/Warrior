using UnityEngine;

public class Charapter : MonoBehaviour, IMovable, IDamageable
{
    public Animator MyAnimator;

    [SerializeField] private float MyMoveSpeed = 0.5f;

    public float MoveSpeed
    {
        get => MyMoveSpeed;
    }

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
    
    public Vector3 Rotation
    {
        get => transform.eulerAngles;
        set  => transform.rotation = Quaternion.Euler(value);
    }
    
    [SerializeField] private float MyRotateSpeed = 100;

    public float RotationSpeed => MyRotateSpeed;

    private float _rotateDirection = 0;

    public float RotationDirection
    {
        get => _rotateDirection;
        set => _rotateDirection = value;
    }

    public bool IsRotating => RotationDirection != 0;
    
    public Transform MyTransform => this.transform;

    protected float Health = 100;

    protected void Awake()
    {
        if (MyAnimator == null)
            MyAnimator = this.GetComponent<Animator>();
    }
    
    void OnMoveChange()
    {
        //Debug.Log("Move change to " + IsMoving.ToString());
        
        if (MyAnimator != null)
            MyAnimator.SetBool("IsMoving", IsMoving);
    }
    
    public bool IsDead => Health <= 0;
    
    public void TakeDamage(float damageAmount)
    {
        Debug.Log(this.name + ": -" + damageAmount + " health points");
        
        Health -= damageAmount;
        Health = Mathf.Clamp(Health, 0, 100);
        
        if (MyAnimator != null)
            MyAnimator.SetTrigger("TakeDamage");

        if (Health <= 0)
            Dead();
    }

    private void Dead()
    {
        if (MyAnimator != null)
            MyAnimator.SetTrigger("IsDead");
    }
}
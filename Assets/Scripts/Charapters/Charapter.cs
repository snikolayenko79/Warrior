using UnityEngine;

public interface IMovable
{
    Vector3 MoveDirection { get; set; }
    Vector3 Orientation { get; }
}

public class Charapter : MonoBehaviour, IMovable
{
    public Animator MyAnimator;

    private Vector3 _moveDirection;

    public Vector3 MoveDirection
    {
        get => _moveDirection;
        set
        {
            bool oldIsMoving = IsMoving;
            
            _moveDirection = value;
            
            if (oldIsMoving != IsMoving)
            {
                OnMoveChange();
            }
        }
    }
    
    public Vector3 Orientation
    {
        // TODO. настраивать в редакторе.
        get => -this.transform.up;
    }
    
    public bool IsMoving { get => _moveDirection != Vector3.zero; }

    protected void Awake()
    {
        if (MyAnimator == null)
            MyAnimator = this.GetComponent<Animator>();
    }

    void Update()
    {
        if (IsMoving)
        {
            transform.position += _moveDirection * Time.deltaTime * 0.3f;
        }
    }
    
    protected void Rotate()
    {
        Debug.Log("Rotate");
    }
    
    protected void Walk()
    {
        Debug.Log("Walk");
    }
    
    protected void Attack()
    {
        Debug.Log("Attack");
    }

    void OnMoveChange()
    {
        Debug.Log("Move change to " + IsMoving.ToString());
        
        if (MyAnimator != null)
            MyAnimator.SetBool("IsMoving", IsMoving);
    }
}

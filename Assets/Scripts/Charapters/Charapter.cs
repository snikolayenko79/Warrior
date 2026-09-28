using UnityEngine;

public class Charapter : MonoBehaviour, IMovable
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
}

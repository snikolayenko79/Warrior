using UnityEngine;

public class Charapter : MonoBehaviour
{
    public Animator MyAnimator;

    protected void Awake()
    {
        if (MyAnimator == null)
            MyAnimator = this.GetComponent<Animator>();
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
}

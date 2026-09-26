using UnityEngine;
using System.Collections.Generic;

public class Player : Charapter, ISpellCaster, IInputListener
{
    public List<string> ActionsName => MyActionsName;
    
    [SerializeField] List<string> MyActionsName;
    
    private IInputRouter  MyInputRouter;
    
    public void Initialize(IInputRouter inputRouter)
    {
        MyInputRouter = inputRouter;
        MyInputRouter.Register(this);
    }
    
    void OnDestroy()
    {
        MyInputRouter.Unregister(this);
    }

    public void Spell()
    {
        if (MyAnimator)
            MyAnimator.SetTrigger("Spell");
    }
    
    public void HandleInput(string actionName, InputContext inputContext)
    {
        if (actionName == "Rotate")
        {
            Rotate();
        }
        
        if (actionName == "Move")
        {
            Walk();
        }
        
        if (actionName == "Attack")
        {
            Attack();
        }
        
        if (actionName == "Spell")
        {
            Spell();
        }
    }

    // void Update()
    // {
    //     Vector3 p = transform.localPosition;
    //     p -= transform.up * Time.deltaTime * 0.1f;
    //     transform.localPosition = p;
    // }
}

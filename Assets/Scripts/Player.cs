using UnityEngine;
using System.Collections.Generic;

public class Player : Charapter, IInputListener
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

    public void HandleInput(string actionName, float val = 0)
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
}

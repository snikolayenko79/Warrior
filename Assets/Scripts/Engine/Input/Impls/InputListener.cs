using UnityEngine;
using System.Collections.Generic; 

public abstract class InputListener : MonoBehaviour, IInputListener
{
    // Наследники переопределят это свойство или настроят в инспекторе
    public virtual List<string> ActionsName => myActionsName;
    
    [SerializeField] private List<string> myActionsName;
    
    protected IInputRouter MyInputRouter { get; private set; }
    private bool _isInitialized;

    public void Initialize(IInputRouter inputRouter)
    {
        MyInputRouter = inputRouter;
        _isInitialized = true;

        if (gameObject.activeInHierarchy)
        {
            MyInputRouter.Register(this);
        }
    }
    
    protected virtual void OnEnable()
    {
        if (_isInitialized && MyInputRouter != null)
        {
            MyInputRouter.Register(this);
        }
    }
    
    protected virtual void OnDisable()
    {
        if (_isInitialized && MyInputRouter != null)
        {
            MyInputRouter.Unregister(this);
        }
    }

    // Ключевой метод интерфейса IInputListener. 
    // abstract заставляет всех наследников обязательно его реализовать.
    public abstract void HandleInput(string actionName, InputContext context);
}

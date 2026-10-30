using UnityEngine;
using System.Collections.Generic;
using Zenject;

public class ShopWindowUI : ContainerWindowUI, IUIView
{
    private IStore _store;
    
    [Inject]
    public void Construct(IStore store)
    {
        _store = store;
        _container = store as IContainer;
    }
    
    protected override void OnSlotUpdate(ContainerSlotUI slot)
    {
        base.OnSlotUpdate(slot);
        
        if (_store.Customer != null && slot.Item is IPurchasable purchasable)
            slot.SetAccessibility(_store.Customer.Coins >= purchasable.Price);
    }
    
    public bool IsModal => true;
    public bool PauseTime => true;
    
    public void SetInteractable(bool isInteractable) {}
    
    public void OnCreate(params object[] args)
    {
        _store.Customer = args[0] as ICustomer;
    }
    
    public void Destroy()
    {
        UnityEngine.Object.Destroy(gameObject);
    }
}

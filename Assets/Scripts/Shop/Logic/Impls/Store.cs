using UnityEngine;
using System.Collections.Generic;
using Zenject;

public class Store : Container, IStore
{
    private List<IPurchasable> _products = new List<IPurchasable>();
    
    public IReadOnlyList<IPurchasable> Products => _products;
    
    private StoreConfig _config;

    [Inject]
    public void Construct(StoreConfig config)
    {
        _config = config;
    }
    
    protected new void Start()
    {
        base.Start();
        
        foreach (var item in _config.Items)
        {
            AddItemToContainer(item);
            _products.Add(item);
        }
    }
    
    protected override void OnItemAdded(IContainerItem item)
    {
        base.OnItemAdded(item);

        if (item is IPurchasable purchasable)
        {
            _products.Add(purchasable);
        }
    }
    
    protected override void OnItemRemoved(IContainerItem item)
    {
        base.OnItemAdded(item);

        if (item is IPurchasable purchasable)
        {
            _products.Remove(purchasable);
        }
    }

    public override void OnInteract(IContainerInteractor interactor, IContainerItem item)
    {
        if (interactor is ICustomer customer && item is IPurchasable purchasable)
        {
            OnPurchase(customer, purchasable);
        }
    }
    
    public void OnPurchase(ICustomer customer, IPurchasable item)
    {
        if (customer.Coins >= item.Price)
        {
            customer.Coins -= item.Price;
            item.OnPurchase(customer);
        }
    }
}
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Zenject;

public class Store : Container, IStore
{
    private List<IPurchasable> _products = new List<IPurchasable>();
    
    public IReadOnlyList<IPurchasable> Products => _products;
    
    new void Awake()
    {
        base.Awake();
        
        foreach (var item in config.Items)
        {
            if (item is IPurchasable purchasable)
                _products.Add(purchasable);
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
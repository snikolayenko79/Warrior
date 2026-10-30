using UnityEngine;
using System.Collections.Generic;
using Zenject;

public class Store : Container, IStore
{
    private List<IPurchasable> _products = new List<IPurchasable>();
    
    public IReadOnlyList<IPurchasable> Products => _products;
    
    private StoreConfig _config;
    private ICustomer _customer;

    [Inject]
    public void Construct(StoreConfig config)
    {
        _config = config;
        Initialize(config.Items);
    }
    
    public override void Initialize(IReadOnlyList<IContainerItem> items)
    {
        if (items == null)
            return;
        
        foreach (var item in items)
        {
            if (item is IPurchasable purchasable)
            {
                AddItemToStore(purchasable);
            }
        }
    }

    public void AddItemToStore(IPurchasable purchasable)
    {
        _products.Add(purchasable);
        
        if (purchasable is IContainerItem containerItem)
            AddItemToContainer(containerItem);
    }

    public void RemoveItemFromStore(IPurchasable purchasable)
    {
        _products.Remove(purchasable);
        
        if (purchasable is IContainerItem containerItem)
            RemoveItemFromContainer(containerItem);
    }

    public ICustomer Customer
    {
        get => _customer;
        set => _customer = value;
    }
    
    public override void OnInteract(IContainerItem item)
    {
        if (Customer != null && item is IPurchasable purchasable)
        {
            OnPurchase(Customer, purchasable);
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
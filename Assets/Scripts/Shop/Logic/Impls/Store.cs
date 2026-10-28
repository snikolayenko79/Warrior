using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Zenject;

public class Store : MonoBehaviour, IStore
{
    private List<IPurchasable> _products = new List<IPurchasable>();
    
    public IReadOnlyList<IPurchasable> Products => _products;

    [Inject]
    public void Construct(StoreConfig config)
    {
        _products.Clear();
        _products.AddRange(config.Products);
    }
    
    public void Purchase(ICustomer customer, IPurchasable item)
    {
        if (customer.Coins >= item.Price)
        {
            customer.Coins -= item.Price;
            item.OnPurchase(customer);
        }
    }
}
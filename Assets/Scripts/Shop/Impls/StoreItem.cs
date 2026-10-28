using UnityEngine;

public abstract class StoreItem : ScriptableObject, IPurchasable
{
    public string name;
    public int price;
    public Sprite icon;

    public string Name => name;
    public int Price => price;
    public Sprite Icon => icon;
    
    public abstract void OnPurchase(ICustomer customer);
}
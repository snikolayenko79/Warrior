using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Shop/New item")]
public class StoreItem : ContainerItem, IPurchasable
{
    public int price;
    public int Price => price;
}
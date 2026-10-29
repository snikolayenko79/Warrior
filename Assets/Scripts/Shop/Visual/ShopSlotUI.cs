using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopSlotUI : ContainerSlotUI
{
    [SerializeField] private TextMeshProUGUI _priceText;
    protected override void OnSlotSetup()
    {
        if (Item is IPurchasable purchasable)
            _priceText.text = "$" + purchasable.Price.ToString();
    }
}
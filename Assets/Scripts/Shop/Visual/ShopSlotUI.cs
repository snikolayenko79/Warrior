using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopSlotUI : MonoBehaviour
{
    [SerializeField] private Image _iconImage;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField] private Button _buyButton;

    private IPurchasable _item;
    public IPurchasable Item => _item;
    
    private ShopWindowUI.OnBuyClickDelegate _onBuyClick;
    
    public void Setup(IPurchasable item, ShopWindowUI.OnBuyClickDelegate OnBuyClick)
    {
        _item = item;
        _onBuyClick = OnBuyClick;
        
        // Заполняем UI элементы данные из IPurchasable
        _nameText.text = item.Name;
        _priceText.text = "$" + item.Price.ToString();
        _iconImage.sprite = item.Icon;

        // Подписываемся на клик кнопки покупки
        _buyButton.onClick.RemoveAllListeners();
        _buyButton.onClick.AddListener(OnBuyClicked);
    }
    
    public void SetAccessibility(bool bAccessible)
    {
        if (_buyButton)
            _buyButton.interactable = bAccessible;
    }
    
    private void OnBuyClicked()
    {
        if (_onBuyClick != null && _item != null)
        {
            _onBuyClick(_item);
        }
    }
}

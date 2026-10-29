using UnityEngine;
using System.Collections.Generic;
using Zenject;

public class ShopWindowUI : UIView
{
    [SerializeField] private Transform _container; // Объект с компонентом Grid/Vertical Layout Group
    [SerializeField] private ShopSlotUI _slotPrefab;

    private IStore _store;
    private ICustomer _customer;
    private readonly List<ShopSlotUI> _activeSlots = new List<ShopSlotUI>();

    public delegate void OnBuyClickDelegate(IPurchasable item);
    
    [Inject]
    public void Construct(IStore store)
    {
        _store = store;
    }
    
    public override void OnCreate(params object[] args)
    {
        _customer = args[0] as ICustomer;
    }
    
    void Start()
    {
        RefreshWindow();
    }

    // Метод для полной отрисовки витрины
    public void RefreshWindow()
    {
        ClearWindow();
        Debug.Log(_store);
        foreach (var item in _store.Products)
        {
            //Debug.Log(item);
            ShopSlotUI newSlot = Instantiate(_slotPrefab, _container);
            
            newSlot.Setup(item, OnBuyClick);
            newSlot.SetAccessibility(_customer.Coins >= item.Price);
            
            _activeSlots.Add(newSlot);
        }
    }

    private void ClearWindow()
    {
        foreach (var slot in _activeSlots)
        {
            if (slot != null) Destroy(slot.gameObject);
        }
        _activeSlots.Clear();
    }
    
    // Полезно вызывать этот метод, например, когда игрок получает монеты в процессе открытого UI, 
    // чтобы кнопки "Купить" динамически становились активными/неактивными
    public void UpdateAllSlotsVisual()
    {
        foreach (var slot in _activeSlots)
        {
            slot.SetAccessibility(_customer.Coins >= slot.Item.Price);
        }
    }

    private void OnBuyClick(IPurchasable item)
    {
        _store.Purchase(_customer, item);
    }
}

using UnityEngine;
using System.Collections.Generic;
using TMPro;
using Zenject;

public class ShopWindowUI : ContainerWindowUI, IUIView
{
    public TextMeshProUGUI coinsText;
    
    private IStore _store;
    
    [Inject]
    public void Construct(IStore store)
    {
        _store = store;
        ItemsContainer = store as IContainer;
    }
    
    protected override void OnSlotCreated(ContainerSlotUI slot)
    {
        base.OnSlotCreated(slot);
        UpdateSlot(slot);
    }

    protected override void OnWindowCreated()
    {
        base.OnWindowCreated();
        
        if (!_store.Customer.IsUnityNull())
            UpdateCoins(_store.Customer);
    }

    public bool IsModal => true;
    public bool PauseTime => true;
    
    public void SetInteractable(bool isInteractable) {}
    
    public void OnCreate(params object[] args)
    {
        _store.Customer = args[0] as ICustomer;
        
        if (isActiveAndEnabled )
            _store.Customer.OnCoinsChanged += OnCoinsChanged;
    }
    
    public void OnEnable()
    {
        if (!_store.Customer.IsUnityNull())
            _store.Customer.OnCoinsChanged += OnCoinsChanged;
    }
    
    public void OnDisable()
    {
        if (!_store.Customer.IsUnityNull())
            _store.Customer.OnCoinsChanged -= OnCoinsChanged;
    }
    
    public void Destroy()
    {
        UnityEngine.Object.Destroy(gameObject);
    }

    private void UpdateSlot(ContainerSlotUI slot)
    {
        if (!_store.Customer.IsUnityNull() && slot.Item is IPurchasable purchasable)
            slot.SetAccessibility(_store.Customer.Coins >= purchasable.Price);
    }

    private void OnCoinsChanged(int oldCoins, int newCoins)
    {
        UpdateWindow();
    }
    
    // Вызывается этот метод, например, когда игрок получает или тратит монеты в процессе открытого UI, 
    // чтобы кнопки "Купить" динамически становились активными/неактивными
    private void UpdateWindow()
    {
        UpdateCoins(_store.Customer);
        
        foreach (var slot in _activeSlots)
            UpdateSlot(slot);
    }
    
    private void UpdateCoins(ICustomer customer)
    {
        if (customer.IsUnityNull())
            return;
        
        if (coinsText)
            coinsText.text = customer.Coins.ToString();
    }
}

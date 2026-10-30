using UnityEngine;
using System.Collections.Generic;
using Zenject;

public class ContainerWindowUI : UIView
{
    [SerializeField] private Transform _layoutGroup; // Объект с компонентом Grid/Vertical Layout Group
    [SerializeField] private ContainerSlotUI _slotPrefab;
    
    protected IContainer _container;
    
    protected IContainerInteractor _interactor;
    private readonly List<ContainerSlotUI> _activeSlots = new List<ContainerSlotUI>();

    public delegate void OnClickDelegate(IContainerItem item);
    
    public override void OnCreate(params object[] args)
    {
        _interactor = args[0] as IContainerInteractor;
    }
    
    void Start()
    {
        RefreshWindow();
    }
    
    public void RefreshWindow()
    {
        ClearWindow();
        
        foreach (var item in _container.Items)
        {
            ContainerSlotUI newSlot = Instantiate(_slotPrefab, _layoutGroup);
            
            newSlot.Setup(item, OnSlotClick);
            
            OnSlotUpdate(newSlot);
            
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
    public void UpdateAllSlots()
    {
        foreach (var slot in _activeSlots)
        {
            OnSlotUpdate(slot);
        }
    }

    private void OnSlotClick(IContainerItem item)
    {
        _container.OnInteract(_interactor, item);
    }
    
    protected virtual void OnSlotUpdate(ContainerSlotUI slot) {}
}
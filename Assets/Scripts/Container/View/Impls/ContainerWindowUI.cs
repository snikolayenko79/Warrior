using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;
using System;
using Zenject;

public class ContainerWindowUI : MonoBehaviour
{
    [SerializeField] private Transform parent; // Объект с компонентом Grid/Vertical Layout Group
    [SerializeField] private ContainerSlotUI slotPrefab;
    
    // TODO. Инициализируется только в классах-наследниках. Т.е. нельзя использовать именно этот класс.
    protected IContainer ItemsContainer;
    
    protected readonly List<ContainerSlotUI> _activeSlots = new List<ContainerSlotUI>();

    public delegate void OnClickDelegate(ContainerSlotUI itemSlot);

    private DiContainer _diContainer;

    [Inject]
    public void Construct(DiContainer diContainer)
    {
        _diContainer = diContainer;
    }
    
    protected void Start()
    {
        RefreshWindow();

        if (!ItemsContainer.IsUnityNull())
        {
            ItemsContainer.OnItemAdded += OnItemAdded;
            ItemsContainer.OnItemRemoved += OnItemRemoved;
        }
    }
    
    protected void OnDestroy()
    {
        if (!ItemsContainer.IsUnityNull())
        {
            ItemsContainer.OnItemAdded -= OnItemAdded;
            ItemsContainer.OnItemRemoved -= OnItemRemoved;
        }
    }
    
    public void RefreshWindow()
    {
        if (ItemsContainer.IsUnityNull())
            return;
        
        ClearWindow();
        
        foreach (var item in ItemsContainer.Items)
        {
            ContainerSlotUI newSlot = _diContainer.InstantiatePrefabForComponent<ContainerSlotUI>(slotPrefab, parent);

            IContainerItemView itemView = null;
            if (item.ItemViewPrefab != null)
                itemView = _diContainer.InstantiatePrefabForComponent<IContainerItemView>(item.ItemViewPrefab, newSlot.transform.parent);
            
            
            newSlot.Setup(item, itemView, OnSlotClick);
            
            OnSlotCreated(newSlot);
            
            _activeSlots.Add(newSlot);
        }
        
        OnWindowCreated();
    }
    
    protected virtual void OnWindowCreated() {}
    protected virtual void OnSlotCreated(ContainerSlotUI slot) {}
    
    private void ClearWindow()
    {
        foreach (var slot in _activeSlots)
        {
            if (slot != null) Destroy(slot.gameObject);
        }
        
        _activeSlots.Clear();
    }
    
    private void OnSlotClick(ContainerSlotUI slot)
    {
        if (slot && !slot.ItemView.IsUnityNull() && !slot.ItemView.InteractLogic.IsUnityNull())
            slot.ItemView.InteractLogic.Execute(ItemsContainer, slot.Item);
    }

    private void OnItemAdded(IContainerItem item)
    {
        RefreshWindow();
    }
    
    private void OnItemRemoved(IContainerItem item)
    {
        RefreshWindow();
    }
}
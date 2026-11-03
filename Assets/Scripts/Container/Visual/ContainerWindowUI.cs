using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;
using Zenject;

public class ContainerWindowUI : MonoBehaviour
{
    [SerializeField] private Transform parent; // Объект с компонентом Grid/Vertical Layout Group
    [SerializeField] private ContainerSlotUI slotPrefab;
    
    protected IContainer ItemsContainer;
    
    private readonly List<ContainerSlotUI> _activeSlots = new List<ContainerSlotUI>();

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
    }
    
    public void RefreshWindow()
    {
        ClearWindow();
        foreach (var item in ItemsContainer.Items)
        {
            ContainerSlotUI newSlot = _diContainer.InstantiatePrefabForComponent<ContainerSlotUI>(slotPrefab, parent);

            IContainerItemView itemView = null;
            if (item.ItemViewPrefab != null)
                itemView = _diContainer.InstantiatePrefabForComponent<IContainerItemView>(item.ItemViewPrefab, newSlot.transform.parent);
            
            
            newSlot.Setup(item, itemView, OnSlotClick);
            
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

    private void OnSlotClick(ContainerSlotUI slot)
    {
        if (slot && !slot.ItemView.IsUnityNull())
            slot.ItemView.OnInteract(ItemsContainer, slot.Item);
    }
    
    protected virtual void OnSlotUpdate(ContainerSlotUI slot) {}
}
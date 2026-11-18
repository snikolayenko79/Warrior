using UnityEngine;
using System;

public class CharacterData : MonoBehaviour, ICharacterData
{
    private IDataContainer _container;

    public IDataContainer Container
    {
        get => _container;
        set => _container = value;
    }
    
    public event Action<IData> OnDataInitializedEvent;
    public event Action<IData> OnDataChangedEvent;

    public void Initialize()
    {
        OnInitialize();
        OnDataInitializedEvent?.Invoke(this);
    }

    public virtual void OnInitialize() { }

    public void RaiseDataChanged()
    {
        OnDataChanged();
        OnDataChangedEvent?.Invoke(this);
    }

    public virtual void OnDataChanged() { }
}
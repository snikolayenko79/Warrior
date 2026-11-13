using UnityEngine;
using System;

public class CharacterData : MonoBehaviour, ICharacterData
{
    public event Action<IData> OnDataInitialized;
    public event Action<IData> OnDataChanged;
    
    public virtual void Initialize()
    {
        RaiseDataInitialized();
    }
    
    protected void RaiseDataInitialized()
    {
        OnDataInitialized?.Invoke(this);
    }
    
    protected void RaiseDataChanged()
    {
        OnDataInitialized?.Invoke(this);
    }
}
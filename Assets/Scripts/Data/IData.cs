using UnityEngine;
using System;

public interface IData
{
    IDataContainer Container { get; set; }
    
    void Initialize();
    void OnInitialize();
    event Action<IData> OnDataInitializedEvent;
    
    void RaiseDataChanged();
    void OnDataChanged();
    event Action<IData> OnDataChangedEvent;
}
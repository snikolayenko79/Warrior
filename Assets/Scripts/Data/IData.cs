using UnityEngine;
using System;

public interface IData
{
    event Action<IData> OnDataInitialized;
    event Action<IData> OnDataChanged;
    
    void Initialize();
}
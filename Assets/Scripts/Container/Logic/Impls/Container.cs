using System;
using UnityEngine;
using System.Collections.Generic;
using Zenject;

public abstract class Container : MonoBehaviour, IContainer
{
    public ContainerConfig config;
    public IReadOnlyList<IContainerItem> Items => config.Items;
    
    public abstract void OnInteract(IContainerInteractor interactor, IContainerItem item);

    protected void Awake() {}
}
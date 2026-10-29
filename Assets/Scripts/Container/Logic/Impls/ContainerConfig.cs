using UnityEngine;
using System.Collections.Generic;

public class ContainerConfig : ScriptableObject
{
    [SerializeField] private List<ContainerItem> _items;
    public IEnumerable<IContainerItem> Items => _items;
}
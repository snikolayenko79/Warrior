using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "InventoryConfig", menuName = "Inventory/Config")]
public class InventoryConfig : ScriptableObject
{
    [SerializeField] private List<InventoryItem> items;
    public IReadOnlyList<InventoryItem> Items => items;
}
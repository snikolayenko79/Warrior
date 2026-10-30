using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "StoreConfig", menuName = "Shop/Config")]
public class StoreConfig  : ScriptableObject
{
    [SerializeField] private List<StoreItem> items;
    public IReadOnlyList<StoreItem> Items => items;
}
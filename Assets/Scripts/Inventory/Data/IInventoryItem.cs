using UnityEngine;

public interface IInventoryItem
{
    string Name { get; }
    bool IsStackable { get; } // Может ли быть несколько таких предметов в инвентаре одновременно
}
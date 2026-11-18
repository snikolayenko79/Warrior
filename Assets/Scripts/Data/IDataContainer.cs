using UnityEngine;

public interface IDataContainer
{
    IData Data  { get; }
    string Name { get; }
    void InitializeData();
}
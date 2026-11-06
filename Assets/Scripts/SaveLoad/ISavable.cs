using UnityEngine;

public interface ISavable
{
    void Save(ISaveLoadService saveService);
    void Load(ISaveLoadService loadService);
}
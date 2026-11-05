using UnityEngine;

public interface ISavable
{
    // Метод, который вызывается глобальным сервисом перед выходом со сцены
    void Save(ISaveLoadService saveService);
}
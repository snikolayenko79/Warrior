using UnityEngine;

public interface IObjectPool
{
    // Получить объект из пула
    GameObject Get(GameObject prefab);
    // Вернуть объект в пул
    void ReturnToPool(GameObject obj);
}
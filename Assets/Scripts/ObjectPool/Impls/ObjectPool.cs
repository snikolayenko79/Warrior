using UnityEngine;
using System.Collections.Generic;
using Zenject;

public class ObjectPool : MonoBehaviour, IObjectPool
{
    private Dictionary<GameObject, Queue<GameObject>> _pooledObjects = new Dictionary<GameObject, Queue<GameObject>>();

    private DiContainer _container;

    // Внедряем сам контейнер Zenject
    [Inject]
    public void Construct(DiContainer container)
    {
        _container = container;
    }
    
    // Получить объект из пула
    public GameObject Get(GameObject prefab)
    {
        if (prefab == null) return null;

        if (!_pooledObjects.ContainsKey(prefab))
        {
            _pooledObjects[prefab] = new Queue<GameObject>();
        }

        GameObject obj;

        if (_pooledObjects[prefab].Count > 0)
        {
            obj = _pooledObjects[prefab].Dequeue();
        }
        else
        {
            obj = _container.InstantiatePrefab(prefab);
            var poolMember = obj.AddComponent<PoolMember>();
            poolMember.OriginPrefab = prefab;
            poolMember.MyPool = this;
        }

        obj.SetActive(true);
        return obj;
    }

    // Вернуть объект в пул
    public void ReturnToPool(GameObject obj)
    {
        if (obj == null) return;

        var poolMember = obj.GetComponent<PoolMember>();
        if (poolMember != null && poolMember.MyPool == this)
        {
            obj.SetActive(false);
            _pooledObjects[poolMember.OriginPrefab].Enqueue(obj);
        }
        else
        {
            Object.Destroy(obj);
        }
    }
}

public class PoolMember : MonoBehaviour
{
    public GameObject OriginPrefab;
    public ObjectPool MyPool;
}
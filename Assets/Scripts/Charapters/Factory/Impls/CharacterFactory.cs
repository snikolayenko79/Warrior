using System;
using UnityEngine;
using System.Collections;

public class CharacterFactory : MonoBehaviour, ICharacterFactory
{
    private IObjectPool _objectPool = null;

    public GameObject SpawnCharapter(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (prefab == null)
            return null;

        if (_objectPool == null)
        {
            gameObject.TryGetComponent<IObjectPool>(out _objectPool);

            if (_objectPool == null)
            {
                Debug.LogError($"На объекте фабрики {gameObject.name} отсутствует компонент наследующий интерфейс IObjectPool!", gameObject);
                return null;
            }
        }

        // 1. Извлекаем объект из пула (или инстанцируем, если пул пуст)
        GameObject enemyInstance = _objectPool.Get(prefab);
        
        enemyInstance.transform.position = position;
        enemyInstance.transform.rotation = rotation;
        
        enemyInstance.transform.SetParent(this.transform, true);

        // 2. Получаем интерфейс движения
        Character character = enemyInstance.GetComponent<Character>();
        if (character == null)
        {
            Debug.LogError($"На префабе {prefab.name} отсутствует компонент Character!");
            return enemyInstance;
        }

        return enemyInstance;
    }
    
    // Публичный метод деспавна (можно вызвать и вручную, например, при деспавне за экраном)
    public void DespawnCharacter(Character character)
    {
        if (character == null || _objectPool == null)
            return;
        
        // 2. Сбрасываем физические параметры перед сном
        character.MoveSpeed = 0;
        character.IsMoving = false;
        character.CurrentPath = null;

        // 3. Возвращаем GameObject в пул объектов
        _objectPool.ReturnToPool(character.gameObject);
    }
}
using UnityEngine;
using System.Collections;

public class CharacterFactory : MonoBehaviour, ICharacterFactory
{
    public CharacterStateController characterStateController;
    public float timeBeforeDespawn = 2.0f; // Время в секундах, пока играет анимация смерти
    
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

        character.OnDead += OnCharapterDead;
        
        // 3. Возвращаем персонажу "жизнь" (сбрасываем здоровье до 100)
        character.TakeDamage(-1000); 

        // 4. Сбрасываем и настраиваем навигацию
        // Если на персонаже висит настроенный в инспекторе INavigationPath, возвращаем его как стартовый компас
        if (enemyInstance.TryGetComponent<INavigationPath>(out var path))
        {
            character.CurrentPath = path;
        }
        else
        {
            // Если патруля нет (например, статичная турель), навигатор по умолчанию отсутствует
            character.CurrentPath = null;
        }

        // 5. Регистрируем сущность в глобальной системе симуляции движения
        if (characterStateController != null)
        {
            characterStateController.RegisterEntity(character);
        }

        return enemyInstance;
    }

    void OnCharapterDead(Character character)
    {
        character.OnDead -= OnCharapterDead;
        
        // 1. Сообщаем контроллеру ИИ, что этого персонажа больше не нужно обновлять
        if (characterStateController)
        {
            characterStateController.UnregisterEntity(character);
        }

        StartCoroutine(DelayedDespawnRoutine((character)));
    }
    
    private IEnumerator DelayedDespawnRoutine(Character charapter)
    {
        // Ждем указанное количество секунд (пока враг красиво падает на землю)
        yield return new WaitForSeconds(timeBeforeDespawn);

        // 4. По истечении таймера окончательно прячем тело в пул
        DespawnCharacter(charapter);
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
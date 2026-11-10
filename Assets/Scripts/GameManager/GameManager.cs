using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zenject;

public class GameManager : MonoBehaviour, IGameManager
{
    public CharacterStateController characterStateController;
    public float timeBeforeDespawn = 2.0f; // Время в секундах, пока играет анимация смерти
    
    private Dictionary<SideType, List<GameObject>> _characters = new  Dictionary<SideType, List<GameObject>>();
    
    private ICharacterFactory _characterFactory;

    private int _gameplayLock = 0;

    [Inject]
    public void Construct(ICharacterFactory characterFactory)
    {
        _characterFactory = characterFactory;
    }
    
    public GameObject CreateCharacter(string name, GameObject prefab, Vector3 position, Quaternion rotation, SideType side)
    {
        GameObject go = _characterFactory.SpawnCharapter(prefab, position, rotation);
        
        if (!_characters.ContainsKey(side))
            _characters[side] = new List<GameObject>();
        
        _characters[side].Add(go);
        
        go.name = name;
        
        // 2. Получаем интерфейс движения
        Character character = go.GetComponent<Character>();
        if (!character.IsUnityNull())
        {
            character.OnDead += OnCharapterDead;
        
            // 3. Возвращаем персонажу "жизнь" (сбрасываем здоровье до 100)
            if (character is IDataContainer dataContainer)
            {
                if (!dataContainer.Data.IsUnityNull())
                    dataContainer.Data.Initialize();
            }

            // 4. Сбрасываем и настраиваем навигацию
            // Если на персонаже висит настроенный в инспекторе INavigationPath, возвращаем его как стартовый компас
            if (go.TryGetComponent<INavigationPath>(out var path))
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
        }
        
        return go;
    }

    public IReadOnlyList<GameObject> GetCharactersBySide(SideType side)
    {
        if (_characters.TryGetValue(side, out List<GameObject> characters))
        {
            return characters;
        }
        
        return System.Array.Empty<GameObject>();
    }
    
    public void SetGameplayLocked(bool bLock)
    {
        if (bLock)
            _gameplayLock++;
        else
            _gameplayLock--;
    }

    public bool IsGameplayLocked()
    {
        return _gameplayLock > 0;
    }
    
    void OnCharapterDead(Character character)
    {
        character.OnDead -= OnCharapterDead;
        
        if (characterStateController != null)
        {
            characterStateController.UnregisterEntity(character);
        }
        
        StartCoroutine(DelayedDespawnRoutine((character)));
    }
    
    private IEnumerator DelayedDespawnRoutine(Character charapter)
    {
        // Ждем указанное количество секунд (пока враг красиво падает на землю)
        yield return new WaitForSeconds(timeBeforeDespawn);
        
        _characterFactory.DespawnCharacter(charapter);
    }
}
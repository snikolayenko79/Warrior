using UnityEngine;
using System.Collections.Generic;
using Zenject;

public class WorldExitTrigger : MonoBehaviour
{
    private ISceneLoader _sceneLoader;
    private ISaveLoadService _saveLoadService;
    private List<ISavable> _savableSystems;
    
    [Inject]
    public void Construct(
        ISaveLoadService saveLoadService, 
        ISceneLoader sceneLoader, 
        List<ISavable> savableSystems) // <- Zenject сам найдет и подставит сюда все ISavable сцены
    {
        _saveLoadService = saveLoadService;
        _sceneLoader = sceneLoader;
        _savableSystems = savableSystems;
    }

    // Вызывается, например, при входе игрока в триггер-коллайдер двери перехода
    private void OnTriggerEnter(Collider other)
    {
        //TODO. Проверить выполнение заданий. Можем ли перейти.
        // Проверяем, что это именно игрок
        if (other.CompareTag("Player"))
        {
            ExecuteLevelTransition();
        }
    }

    public void ExecuteLevelTransition()
    {
        // Опрашиваем по очереди профиль и инвентарь. 
        // Каждый класс запишет свои данные в глобальный контейнер сохранения.
        foreach (ISavable system in _savableSystems)
        {
            system.Save(_saveLoadService);
        }

        Debug.Log("[LevelExitTrigger] Все локальные системы успешно сохранили свое состояние.");

        // Загружаем следующий уровень
        _sceneLoader.LoadScene("World2"); 
    }
}
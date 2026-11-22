using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zenject;
using Scenario;

public class TransitionTrigger : MonoBehaviour
{
    [SerializeField] private string MoveToSceneName = "";
    [SerializeField] private Dialogue CantLeaveVillagers = null;
    [SerializeField] private Dialogue CantLeaveMonsters = null;
    [SerializeField] private Collider Blocker = null;

    private ISceneLoader _sceneLoader;
    private ISaveLoadService _saveLoadService;
    private List<ISavable> _savableSystems;
    
    private ScenarioManager _scenarioManager;
    private IDialogManager _dialogueManager;
    
    [Inject]
    public void Construct(
        ScenarioManager scenarioManager,
        IDialogManager dialogueManager,
        ISaveLoadService saveLoadService, 
        ISceneLoader sceneLoader, 
        List<ISavable> savableSystems) // <- Zenject сам найдет и подставит сюда все ISavable сцены
    {
        _saveLoadService = saveLoadService;
        _sceneLoader = sceneLoader;
        _savableSystems = savableSystems;
        _scenarioManager = scenarioManager;
        _dialogueManager = dialogueManager;
    }

    // Вызывается, например, при входе игрока в триггер-коллайдер двери перехода
    private void OnTriggerEnter(Collider other)
    {
        //Проверяем выполнение заданий. Можем ли перейти.
        if (_scenarioManager.IsUnityNull())
            return;
            
        if (!_scenarioManager.GetBool("AllEnemiesDie"))
        {
            _dialogueManager.StartDialogue(CantLeaveMonsters);
            return;
        }

        if (!_scenarioManager.GetBool("VillagerInteract") || !_scenarioManager.GetBool("SellerInteract"))
        {
            _dialogueManager.StartDialogue(CantLeaveVillagers);
            return;
        }

        // Проверяем, что это именно игрок
        if (other.CompareTag("Player"))
        {
            other.gameObject.TryGetComponent<Player>(out Player player);
            ExecuteLevelTransition(player);
        }
    }

    private void ExecuteLevelTransition(Player player)
    {
        // Опрашиваем по очереди профиль и инвентарь. 
        // Каждый класс запишет свои данные в глобальный контейнер сохранения.
        //foreach (ISavable system in _savableSystems)
        //{
        //    system.Save();
        //}

        //Debug.Log("[LevelExitTrigger] Переход...");
        
        if (Blocker)
            Blocker.gameObject.SetActive(false);

        // Загружаем следующий уровень
        _sceneLoader.LoadScene(MoveToSceneName); 
    }
}
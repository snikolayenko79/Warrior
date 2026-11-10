using Scenario;
using UnityEngine;
using Zenject;

public class World1Installer : MonoInstaller
{
    [Header("Менеджер игры.")]
    [SerializeField] private GameManager gameManager;
    
    [Header("Фабрика персонажей.")]
    [SerializeField] private CharacterFactory characterFactory;
    
    [Header("Контроллер персонажей основной.")]
    [SerializeField] private CharacterStateController _characterStateController;
    
    [Header("Контроллер атак.")]
    [SerializeField] private AttackController _attackController;
    
    [Header("Контроллер заклинаний.")]
    [SerializeField] private SpellController _spellController;
    
    [Header("Поведение - ввод для игрока.")]
    [SerializeField] private BehaviourPlayerInput _behaviourPlayerInput;
    [SerializeField] private BehaviourPlayerInteract _behaviourPlayerInteract;
    
    [Header("Поведение - атаковать игрока.")]
    [SerializeField] private BehaviourAttackPlayer _behaviourAttackPlayer;
    
    [Header("Поведение - следовать за игроком.")]
    [SerializeField] private BehaviourFollowPlayer _behaviourFollowPlayer;
    
    [Header("Поведение - идти по заданному пути.")]
    [SerializeField] private BehaviourFollowPath _behaviourFollowPath;
    
    [Header("Инвентарь")]
    [SerializeField] private InventoryConfig inventoryConfig;
    
    [Header("Магазин")]
    [SerializeField] private StoreConfig storeConfig;
    
    [Header("Сценарий")]
    [SerializeField] private ScenarioData scenarioData;

    public override void InstallBindings()
    {
        Container.Bind<IGameManager>().FromInstance(gameManager);
        Container.Bind<ICharacterFactory>().FromInstance(characterFactory);
        Container.Bind<CharacterStateController>().FromInstance(_characterStateController);
        Container.Bind<IAttackController>().FromInstance(_attackController);
        Container.Bind<SpellController>().FromInstance(_spellController);
        Container.Bind<BehaviourPlayerInput>().FromInstance(_behaviourPlayerInput);
        Container.Bind<BehaviourPlayerInteract>().FromInstance(_behaviourPlayerInteract);
        Container.Bind<BehaviourAttackPlayer>().FromInstance(_behaviourAttackPlayer);
        Container.Bind<BehaviourFollowPlayer>().FromInstance(_behaviourFollowPlayer);
        Container.Bind<BehaviourFollowPath>().FromInstance(_behaviourFollowPath);

        Container.BindInstance(inventoryConfig).AsSingle();
        Container.Bind(typeof(IInventory), typeof(ISavable)).To<Inventory>().AsSingle();
        
        Container.BindInstance(storeConfig).AsSingle();
        Container.Bind(typeof(IStore)).To<Store>().AsSingle();
        
        Container.BindInterfacesAndSelfTo<ScenarioManager>().AsSingle().NonLazy();
        Container.Bind<ScenarioData>().FromInstance(scenarioData);
    }
}
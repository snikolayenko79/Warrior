using UnityEngine;
using Zenject;

public class World1Installer : MonoInstaller
{
    [Header("Менеджер игры.")]
    [SerializeField] private GameManager gameManager;
    
    [Header("Фабрика персонажей.")]
    [SerializeField] private CharacterFactory characterFactory;
    
    [Header("Контроллер атак.")]
    [SerializeField] private AttackController _attackController;
    
    [Header("Контроллер заклинаний.")]
    [SerializeField] private SpellController _spellController;
    
    [Header("Поведение - атаковать игрока.")]
    [SerializeField] private BehaviourAttackPlayer _behaviourAttackPlayer;
    
    [Header("Поведение - следовать за игроком.")]
    [SerializeField] private BehaviourFollowPlayer _behaviourFollowPlayer;
    
    [Header("Инвентарь")]
    [SerializeField] private InventoryConfig inventoryConfig;
    
    [Header("Магазин")]
    [SerializeField] private StoreConfig storeConfig;

    public override void InstallBindings()
    {
        Container.Bind<IGameManager>().FromInstance(gameManager);
        Container.Bind<ICharacterFactory>().FromInstance(characterFactory);
        Container.Bind<IAttackController>().FromInstance(_attackController);
        Container.Bind<SpellController>().FromInstance(_spellController);
        Container.Bind<BehaviourAttackPlayer>().FromInstance(_behaviourAttackPlayer);
        Container.Bind<BehaviourFollowPlayer>().FromInstance(_behaviourFollowPlayer);

        Container.BindInstance(inventoryConfig).AsSingle();
        Container.Bind(typeof(IInventory), typeof(ISavable)).To<Inventory>().AsSingle();
        
        Container.BindInstance(storeConfig).AsSingle();
        Container.Bind<IStore>().To<Store>().AsSingle();
    }
}
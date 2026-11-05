using UnityEngine;
using Zenject;

public class World1Installer : MonoInstaller
{
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
        Container.Bind<IAttackController>().FromInstance(_attackController);
        Container.Bind<SpellController>().FromInstance(_spellController);
        Container.Bind<BehaviourAttackPlayer>().FromInstance(_behaviourAttackPlayer);
        Container.Bind<BehaviourFollowPlayer>().FromInstance(_behaviourFollowPlayer);
        
        Container.BindInstance(inventoryConfig).AsSingle();
        Container.BindInterfacesAndSelfTo<Inventory>().AsSingle();
        
        Container.BindInstance(storeConfig).AsSingle();
        Container.BindInterfacesAndSelfTo<Store>().AsSingle();
    }
}
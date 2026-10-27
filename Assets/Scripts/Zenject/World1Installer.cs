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

    public override void InstallBindings()
    {
        Container.Bind<AttackController>().FromInstance(_attackController);
        Container.Bind<SpellController>().FromInstance(_spellController);
        Container.Bind<BehaviourAttackPlayer>().FromInstance(_behaviourAttackPlayer);
        Container.Bind<BehaviourFollowPlayer>().FromInstance(_behaviourFollowPlayer);
    }
}
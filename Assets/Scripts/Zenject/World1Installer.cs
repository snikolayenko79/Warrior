using UnityEngine;
using Zenject;

public class World1Installer : MonoInstaller
{
    [Header("Поведение - атаковать игрока.")]
    [SerializeField] private BehaviourAttackPlayer _behaviourAttackPlayer;
    
    [Header("Поведение - следовтаь за игроком.")]
    [SerializeField] private BehaviourFollowPlayer _behaviourFollowPlayer;

    public override void InstallBindings()
    {
        Container.Bind<BehaviourAttackPlayer>().FromInstance(_behaviourAttackPlayer);
        Container.Bind<BehaviourFollowPlayer>().FromInstance(_behaviourFollowPlayer);
    }
}
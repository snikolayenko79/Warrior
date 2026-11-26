using Scenario;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class World1Installer : MonoInstaller
{
    [Header("Дверь жителя, который дает заклинание.")]
    [SerializeField] private Door doorVillager;
    
    [Header("Дверь жителя - торговца.")]
    [SerializeField] private Door doorSeller;
    
    [Header("Блокиратор выхода из уровня.")]
    [SerializeField] private Collider exitBlocker;
    
    public override void InstallBindings()
    {
        Container.Bind<Door>().WithId("doorVillager").FromInstance(doorVillager);
        Container.Bind<Door>().WithId("doorSeller").FromInstance(doorSeller);
        
        Container.Bind<Collider>().FromInstance(exitBlocker);
    }
}
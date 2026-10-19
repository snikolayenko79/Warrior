using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        // Здесь будут наши биндинги, например:
        // Container.Bind<GameSceneLoader>().AsSingle();
    }
}
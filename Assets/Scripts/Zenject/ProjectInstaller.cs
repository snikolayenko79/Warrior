using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System;

[Serializable]
public struct SceneUIConfig
{
    public string SceneName;
    public UIView uiPrefab;
}

public class ProjectInstaller : MonoInstaller
{
    [Header("Настройки Экранов Сцен")]
    [SerializeField] private List<SceneUIConfig> sceneUIConfigs;

    [Header("Префаб окна паузы")]
    [SerializeField] private UIView pauseMenuPrefab;

    public override void InstallBindings()
    {
         Container.Bind<CoroutineRunner>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
         Container.Bind<GameSceneLoader>().AsSingle().NonLazy();
        
        // 1. Биндим префаб паузы как чистый интерфейс IUIView
        IUIView pauseInterface = pauseMenuPrefab as IUIView;
        if (pauseInterface == null)
        {
            Debug.LogError("[ProjectInstaller] На префабе паузы отсутствует компонент IUIView!");
        }
        Container.Bind<IUIView>().WithId("PauseMenuPrefab").FromInstance(pauseInterface);
        
        // 2. Формируем строго интерфейсный словарь
        var uiDictionary = new Dictionary<string, IUIView>();
        foreach (var config in sceneUIConfigs)
        {
            if (!string.IsNullOrEmpty(config.SceneName) && config.uiPrefab != null)
            {
                uiDictionary[config.SceneName] = config.uiPrefab;
            }
            else if (config.uiPrefab == null)
            {
                Debug.LogError($"[ProjectInstaller] Префаб для сцены {config.SceneName} не реализует интерфейс IUIView!");
            }
        }
        
        // 3. Передаем словарь интерфейсов в UIManager
        Container.BindInterfacesAndSelfTo<UIManager>()
            .AsSingle()
            .WithArguments(uiDictionary)
            .NonLazy();
    }
}
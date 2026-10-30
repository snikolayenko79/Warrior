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
    
    [Header("Префаб окна диалогов")]
    [SerializeField] private UIView dialoguePrefab;
    
    [Header("Магазин")]
    [SerializeField] private StoreConfig storeConfig;
    [SerializeField] private Store storeInstance;
    
    [Header("Инвентарь")]
    [SerializeField] private InventoryConfig inventoryConfig;
    [SerializeField] private Inventory inventoryInstance;
    
    public override void InstallBindings()
    {
         Container.Bind<CoroutineRunner>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
         Container.Bind<GameSceneLoader>().AsSingle().NonLazy();
         
         Container.Bind<IUIViewFactory>().To<UIViewFactory>().AsSingle();
        
        // 1. Биндим префаб паузы как чистый интерфейс IUIView
        IUIView pauseInterface = pauseMenuPrefab as IUIView;
        if (pauseInterface == null)
        {
            Debug.LogError("[ProjectInstaller] На префабе паузы отсутствует компонент IUIView!");
        }
        Container.Bind<GameObject>().WithId("PauseMenuPrefab").FromInstance(pauseMenuPrefab.gameObject);
        
        IUIView dialogueInterface = dialoguePrefab as IUIView;
        if (dialogueInterface == null)
        {
            Debug.LogError("[ProjectInstaller] На префабе диалогов отсутствует компонент IUIView!");
        }
        
        Container.Bind<GameObject>().WithId("DialoguePrefab").FromInstance(dialoguePrefab.gameObject);
        
        // 2. Формируем строго интерфейсный словарь
        var uiDictionary = new Dictionary<string, GameObject>();
        foreach (var config in sceneUIConfigs)
        {
            if (!string.IsNullOrEmpty(config.SceneName) && config.uiPrefab != null)
            {
                uiDictionary[config.SceneName] = config.uiPrefab.gameObject;
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
        
        Container.BindInterfacesAndSelfTo<DialogueManager>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<DialogueActorsRegistry>().AsSingle().NonLazy();

        Container.BindInstance(storeConfig).AsSingle();
        Container.Bind<Store>().FromComponentInNewPrefab(storeInstance).AsSingle();
        Container.BindInstance(inventoryConfig).AsSingle();
        Container.BindInterfacesAndSelfTo<Inventory>().FromComponentInNewPrefab(inventoryInstance).AsSingle();
    }
}
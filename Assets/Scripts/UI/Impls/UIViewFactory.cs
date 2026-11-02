using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class UIViewFactory : IUIViewFactory
{
    private readonly DiContainer _projectContainer;
    private readonly SceneContextRegistry _sceneContextRegistry;
    
    public UIViewFactory(DiContainer projectContainer, SceneContextRegistry sceneContextRegistry)
    {
        _projectContainer = projectContainer;
        _sceneContextRegistry = sceneContextRegistry;
    }
    
    // Возвращает созданный UI-компонент
    public IUIView CreateView(GameObject prefab, Transform parent, params object[] args)
    {
        if (!prefab)
        {
            Debug.LogError("[UIViewFactory] Попытка создать UI из пустого префаба!");
            return null;
        }

        string activeSceneName = SceneManager.GetActiveScene().name;
        SceneContext currentSceneContext = _sceneContextRegistry.TryGetSceneContextForScene(activeSceneName);
        DiContainer targetContainer = currentSceneContext ? currentSceneContext.Container : _projectContainer;

        IUIView view = targetContainer.InstantiatePrefabForComponent<IUIView>(prefab, parent);
        view.OnCreate(args);
        return view;
    }
}
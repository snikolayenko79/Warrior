using UnityEngine;
using Zenject;

public class UIViewFactory : IUIViewFactory
{
    private DiContainer _container;
    
    public UIViewFactory(DiContainer container)
    {
        _container = container;
    }
    
    // Возвращает созданный UI-компонент
    public IUIView CreateView(GameObject prefab, Transform parent, params object[] args)
    {
        IUIView view = _container.InstantiatePrefabForComponent<IUIView>(prefab);
        view.OnCreate(args);
        return view;
    }
}
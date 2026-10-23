using UnityEngine;
using Zenject;

public interface IUIViewFactory
{
    // Возвращает созданный UI-компонент
    IUIView CreateView(GameObject prefab, Transform parent, params  object[] args);
}
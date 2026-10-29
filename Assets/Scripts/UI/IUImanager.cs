using UnityEngine;

public interface IUIManager
{
    /// <summary>
    /// Полностью очищает текущий экран и всю историю открытых окон (стек).
    /// Обычно вызывается автоматически при смене сцен.
    /// </summary>
    void ClearCurrentUI();

    IUIView CreateView(GameObject popupPrefab, params object[] args);
    
    /// <summary>
    /// Открывает новое окно (попап) поверх текущего экрана.
    /// Предыдущий экран автоматически блокируется (опционально) и сохраняется в историю.
    /// </summary>
    /// <param name="popupPrefab">Префаб окна, содержащий компонент UIScreenView.</param>
    /// <returns>Экземпляр созданного окна типа IUIView.</returns>
    IUIView OpenWindow(GameObject popupPrefab, params object[] args);

    /// <summary>
    /// Закрывает текущее активное верхнее окно. 
    /// Если в истории (стеке) есть предыдущие окна, они автоматически разблокируются.
    /// </summary>
    void CloseActiveWindow();
}
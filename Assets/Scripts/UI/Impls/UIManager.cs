using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class UIManager : IUIManager, IInitializable, IDisposable
{
    private readonly IUIViewFactory _factory;
    private readonly GameSceneLoader _sceneLoader;
    private readonly Dictionary<string, GameObject> _uiConfigs;
    
    private readonly Stack<IUIView> _screenStack = new Stack<IUIView>();
    private IUIView _activeWindow;
    private string _currentSceneName;

    public UIManager(
        IUIViewFactory factory, 
        GameSceneLoader sceneLoader, 
        Dictionary<string, GameObject> uiConfigs)
    {
        _factory = factory;
        _sceneLoader = sceneLoader;
        _uiConfigs = uiConfigs;
    }

    public void Initialize()
    {
        _sceneLoader.OnSceneLoaded += OnSceneLoaded;

        string startSceneName = _sceneLoader.GetActiveSceneName();
        if (!string.IsNullOrEmpty(startSceneName))
        {
            OnSceneLoaded(startSceneName);
        }
    }

    public void Dispose()
    {
        _sceneLoader.OnSceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(string sceneName)
    {
        _currentSceneName = sceneName;
        CheckAndSpawnUI(sceneName);
    }

    private void CheckAndSpawnUI(string sceneName)
    {
        ClearCurrentUI();

        if (_uiConfigs.TryGetValue(sceneName, out GameObject prefab))
        {
            _activeWindow = _factory.CreateView(prefab, null);
            
            // TODO
            if (sceneName.Contains("Menu"))
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }

    public IUIView CreateView(GameObject popupPrefab, params object[] args)
    {
        return _factory.CreateView(popupPrefab, null, args);
    }
    
    public IUIView OpenWindow(GameObject popupPrefab, params object[] args)
    {
        // TODO. переделать IsModal и timeScale. Могут динамически меняться в старт окна.
        // Может в окнах менять. Или вообще отказаться. 
        
        if (popupPrefab == null)
        {
            Debug.LogError("[UIManager] Попытка открыть пустой префаб окна!");
            return null;
        }

        if (_activeWindow != null)
        {
            _screenStack.Push(_activeWindow);
        }

        IUIView newView = CreateView(popupPrefab, args);
        
        if (newView.IsModal)
            _activeWindow.SetInteractable(false);
            
        // Если новое окно требует паузу — останавливаем время
        if (newView.PauseTime)
            Time.timeScale = 0f;

        _activeWindow = newView;
        
        return _activeWindow;
    }

    public void CloseActiveWindow()
    {
        if (_activeWindow == null)
            return;

        // Запоминаем, было ли закрывающееся окно модальным, чтобы знать, разблокировать ли нижнее
        bool closedWasModal = _activeWindow.IsModal;

        // Уничтожаем текущий попап
        _activeWindow.Destroy();
        _activeWindow = null;

        // Если в истории (стеке) что-то есть — достаем предыдущее окно
        if (_screenStack.Count > 0)
        {
            _activeWindow = _screenStack.Pop();
        
            // Возвращаем интерактивность нижнему окну, только если закрытое окно его блокировало
            if (closedWasModal)
                _activeWindow.SetInteractable(true);
        }

        // Если на экране осталось окно, и оно ТРЕБУЕТ паузу
        if (_activeWindow != null && _activeWindow.PauseTime)
        {
            Time.timeScale = 0f;
        }
        else
        {
            // Если окон на экране больше нет (вернулись к чистому HUD игры) 
            // или оставшееся окно НЕ требует паузы — пускаем время (1f)
            Time.timeScale = 1f;
        }
    }

    public void ClearCurrentUI()
    {
        if (_activeWindow != null)
        {
            _activeWindow.Destroy();
        }

        while (_screenStack.Count > 0)
        {
            var screen = _screenStack.Pop();
            if (screen != null)
            {
                screen.Destroy();
            }
        }

        _activeWindow = null;
        Time.timeScale = 1f; // Гарантируем сброс паузы при смене сцены
    }
}
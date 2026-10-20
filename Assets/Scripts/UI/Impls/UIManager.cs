using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class UIManager : IUIManager, IInitializable, IDisposable
{
    private readonly DiContainer _container;
    private readonly GameSceneLoader _sceneLoader;
    private readonly Dictionary<string, IUIView> _uiConfigs;
    
    private readonly Stack<IUIView> _screenStack = new Stack<IUIView>();
    private IUIView _currentScreen;
    private string _currentSceneName;

    public UIManager(
        DiContainer container, 
        GameSceneLoader sceneLoader, 
        Dictionary<string, IUIView> uiConfigs)
    {
        _container = container;
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

        if (_uiConfigs.TryGetValue(sceneName, out IUIView prefab))
        {
            _currentScreen = _container.InstantiatePrefabForComponent<IUIView>(prefab.GameObject);
            
            if (sceneName.Contains("Menu"))
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }
    
    public IUIView OpenPopup(IUIView popupPrefab, params object[] args)
    {
        if (popupPrefab == null)
        {
            Debug.LogError("[UIManager] Попытка открыть пустой префаб окна!");
            return null;
        }

        if (_currentScreen != null)
        {
            // Если новое окно модальное — выключаем кликабельность у старого окна
            if (popupPrefab.IsModal)
                _currentScreen.SetInteractable(false);
            
            _screenStack.Push(_currentScreen);
        }

        _currentScreen = _container.InstantiatePrefabForComponent<IUIView>(popupPrefab.GameObject, (Transform)null, args);
        
        // Если новое окно требует паузу — останавливаем время
        if (_currentScreen.PauseTime)
            Time.timeScale = 0f;
        
        return _currentScreen;
    }

    public void CloseCurrentPopup()
    {
        if (_currentScreen == null)
            return;

        // Запоминаем, было ли закрывающееся окно модальным, чтобы знать, разблокировать ли нижнее
        bool closedWasModal = _currentScreen.IsModal;

        // Уничтожаем текущий попап
        UnityEngine.Object.Destroy(_currentScreen.GameObject);
        _currentScreen = null;

        // Если в истории (стеке) что-то есть — достаем предыдущее окно
        if (_screenStack.Count > 0)
        {
            _currentScreen = _screenStack.Pop();
        
            // Возвращаем интерактивность нижнему окну, только если закрытое окно его блокировало
            if (closedWasModal)
                _currentScreen.SetInteractable(true);
        }

        // Если на экране осталось окно, и оно ТРЕБУЕТ паузу
        if (_currentScreen != null && _currentScreen.PauseTime)
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
        if (_currentScreen != null)
        {
            UnityEngine.Object.Destroy(_currentScreen.GameObject);
        }

        while (_screenStack.Count > 0)
        {
            var screen = _screenStack.Pop();
            if (screen != null)
            {
                UnityEngine.Object.Destroy(screen.GameObject);
            }
        }

        _currentScreen = null;
        Time.timeScale = 1f; // Гарантируем сброс паузы при смене сцены
    }
}
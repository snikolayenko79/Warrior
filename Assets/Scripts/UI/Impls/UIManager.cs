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

    // Конструктор принимает только интерфейсы
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
            // Спавним префаб, передавая его GameObject из интерфейса
            _currentScreen = _container.InstantiatePrefabForComponent<IUIView>(prefab.GameObject);
            
            if (sceneName.Contains("Menu"))
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }
    
    public IUIView OpenPopup(IUIView popupPrefab)
    {
        if (popupPrefab == null)
        {
            Debug.LogError("[UIManager] Попытка открыть пустой префаб окна!");
            return null;
        }

        if (_currentScreen != null)
        {
            _currentScreen.SetInteractable(false);
            _screenStack.Push(_currentScreen);
        }

        // Спавним префаб, забирая GameObject через контракт интерфейса IUIView
        _currentScreen = _container.InstantiatePrefabForComponent<IUIView>(popupPrefab.GameObject);
        return _currentScreen;
    }

    public void CloseCurrentPopup()
    {
        if (_screenStack.Count > 0)
        {
            if (_currentScreen == null) return;

            UnityEngine.Object.Destroy(_currentScreen.GameObject);
            
            _currentScreen = _screenStack.Pop();
            _currentScreen.SetInteractable(true);
            return;
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
    }
}
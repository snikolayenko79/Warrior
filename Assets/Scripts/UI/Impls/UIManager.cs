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
    private IUIView _currentPopup;
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
            _currentPopup = _container.InstantiatePrefabForComponent<UIView>((prefab as UIView).gameObject);
            
            if (sceneName.Contains("Menu"))
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }
    
    public IUIView OpenPopup(GameObject popupPrefab, params object[] args)
    {
        // TODO. переделать IsModal и timeScale. Могут динамически меняться в старт окна.
        // Может в окнах менять. Или вообще отказаться. 
        
        if (popupPrefab == null)
        {
            Debug.LogError("[UIManager] Попытка открыть пустой префаб окна!");
            return null;
        }

        if (_currentPopup != null)
        {
            _screenStack.Push(_currentPopup);
        }

        IUIView viewInPrefab = popupPrefab.GetComponent<IUIView>();
        
        IUIView newView = viewInPrefab.Create(popupPrefab, args);
        
        if (newView.IsModal)
            _currentPopup.SetInteractable(false);
            
        // Если новое окно требует паузу — останавливаем время
        if (newView.PauseTime)
            Time.timeScale = 0f;

        _currentPopup = newView;
        
        return _currentPopup;
    }

    public void CloseCurrentPopup()
    {
        if (_currentPopup == null)
            return;

        // Запоминаем, было ли закрывающееся окно модальным, чтобы знать, разблокировать ли нижнее
        bool closedWasModal = _currentPopup.IsModal;

        // Уничтожаем текущий попап
        _currentPopup.Destroy();
        _currentPopup = null;

        // Если в истории (стеке) что-то есть — достаем предыдущее окно
        if (_screenStack.Count > 0)
        {
            _currentPopup = _screenStack.Pop();
        
            // Возвращаем интерактивность нижнему окну, только если закрытое окно его блокировало
            if (closedWasModal)
                _currentPopup.SetInteractable(true);
        }

        // Если на экране осталось окно, и оно ТРЕБУЕТ паузу
        if (_currentPopup != null && _currentPopup.PauseTime)
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
        if (_currentPopup != null)
        {
            _currentPopup.Destroy();
        }

        while (_screenStack.Count > 0)
        {
            var screen = _screenStack.Pop();
            if (screen != null)
            {
                screen.Destroy();
            }
        }

        _currentPopup = null;
        Time.timeScale = 1f; // Гарантируем сброс паузы при смене сцены
    }
}
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Zenject;

public class GameSceneLoader : IInitializable, ISceneLoader
{
    private readonly DiContainer _container;
    private readonly CoroutineRunner _coroutineRunner;

    // Наше родное событие для UIManager
    public event Action<string> OnSceneLoaded;

    public GameSceneLoader(
        DiContainer container,
        CoroutineRunner coroutineRunner)
    {
        _container = container;
        _coroutineRunner = coroutineRunner;
    }
    
    public void Initialize()
    {
        var activeScene = SceneManager.GetActiveScene();
        
        if (!activeScene.IsValid() || string.IsNullOrEmpty(activeScene.name))
        {
            SceneManager.sceneLoaded += OnUnitySceneLoaded;
        }
    }

    // Метод для безопасного получения имени при старте
    public string GetActiveSceneName()
    {
        var activeScene = SceneManager.GetActiveScene();
        
        if (activeScene.IsValid() && !string.IsNullOrEmpty(activeScene.name))
        {
            return activeScene.name;
        }

        return string.Empty; 
    }

    private void OnUnitySceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnUnitySceneLoaded; // Сразу отписываемся
        OnSceneLoaded?.Invoke(scene.name); // Стреляем нашим событием для UIManager
    }

    // Универсальный метод для будущих переключений (остается без изменений)
    public void LoadScene(string sceneName)
    {
        _coroutineRunner.StartCoroutine(LoadSceneRoutine(sceneName));
    }

    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false; 

        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            
            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }
            
            yield return null;
        }

        // Пропускаем один кадр. За этот кадр новая сцена полностью 
        // активируется, и локальный SceneContext уровня УСПЕЕТ проинициализироваться!
        yield return null; 
        
        // Вызываем событие при ручном переключении сцен
        OnSceneLoaded?.Invoke(sceneName);
    }
}
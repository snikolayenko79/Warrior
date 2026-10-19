using UnityEngine;
using System;

public interface ISceneLoader
{
    void LoadScene(string sceneName);
    event Action<string> OnSceneLoaded;
    string GetActiveSceneName();
}
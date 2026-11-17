#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.SceneManagement;

public static class EditorProfileAutoLoader
{
    // Срабатывает в редакторе ДО вызова любого Awake() на открытой сцене
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void EnsureProfileLoadedInEditor()
    {
        // Если мы запустили игру со стартовой сцены Bootstrap — ничего не делаем,
        // она сама загрузит профиль асинхронно.
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            return;
        }

        if (!Profile.IsCommonLoaded)
            Profile.LoadCommonSync();
        
        if (!Profile.IsPlayerLoaded)
            Profile.LoadPlayerSync(Profile.ProfileKeys.DEFAULT_PLAYER_NAME);
    }
}
#endif
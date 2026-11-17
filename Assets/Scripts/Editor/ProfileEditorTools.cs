#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

public static class ProfileEditorTools
{
    [MenuItem("Tools/Profile/Open profiles folder")]
    public static void OpenSaveFolder()
    {
        EditorUtility.RevealInFinder(Profile.GetProfileFolder());
        Debug.Log(Profile.GetProfileFolder());
    }

    [MenuItem("Tools/Profile/Reset player profile")]
    public static void ResetPlayerProfile()
    {
        _ = Profile.ResetPlayerAsync();
    }
    
    [MenuItem("Tools/Profile/Save player profile")]
    public static void SavePlayerProfile()
    {
        ISavable[] savables = GameObject.FindObjectsByType<PlayerData>(FindObjectsSortMode.None);
        foreach (var savable in savables)
        {
            savable.Save();
        }

        _ = Profile.SavePlayerAsync();
    }
    
    // [MenuItem("Tools/Profile/Reset player profile")]
    // public static void DeleteSaveFile()
    // {
    //     string path = Path.Combine(Application.persistentDataPath, "PlayerProfile.json");
    //     if (File.Exists(path))
    //     {
    //         File.Delete(path);
    //         Debug.Log("Файл сохранения удален.");
    //     }
    //     else
    //     {
    //         Debug.Log("Файл сохранения не найден.");
    //     }
    // }
}
#endif
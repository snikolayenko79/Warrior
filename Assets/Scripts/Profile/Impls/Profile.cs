using UnityEngine;
using System.Threading.Tasks;

public static class Profile
{
    private const string COMMON_FILE_NAME = "CommonProfile";
    private const string PLAYER_FILE_PREFIX = "PlayerProfile_";
    
    private static readonly IProfileSaver Saver = new JsonProfileSaver();

    private static readonly IProfileDataStorage<ProfileData> DataStorage = new ProfileDataStorage<ProfileData>();

    private static string _currentPlayerId = "Default";

    public static bool IsCommonLoaded { get; private set; }
    public static bool IsPlayerLoaded { get; private set; }
    
    public static async Task LoadCommonAsync()
    {
        DataStorage.CommonData = await Saver.LoadAsync<ProfileData>(COMMON_FILE_NAME);
        IsCommonLoaded = true;
    }

    public static async Task LoadPlayerAsync(string playerId)
    {
        _currentPlayerId = playerId;
        string fileName = $"{PLAYER_FILE_PREFIX}{playerId}";
        
        DataStorage.PlayerData = await Saver.LoadAsync<ProfileData>(fileName);
        IsPlayerLoaded = true;
    }
    
    public static async Task SaveCommonAsync()
    {
        await Saver.SaveAsync(COMMON_FILE_NAME, DataStorage.CommonData);
    }

    public static async Task SavePlayerAsync()
    {
        string fileName = $"{PLAYER_FILE_PREFIX}{_currentPlayerId}";
        await Saver.SaveAsync(fileName, DataStorage.PlayerData);
    }

    // Параллельное сохранение обоих профилей
    public static async Task SaveAllAsync()
    {
        await Task.WhenAll(SaveCommonAsync(), SavePlayerAsync());
    }

    public static async Task ResetPlayerAsync()
    {
        DataStorage.PlayerData.Clear();
        await SavePlayerAsync();
    }

    public static void SetVar<T>(string varName, T varValue, bool bCommon = false)
    {
        IProfileData pData = bCommon ? DataStorage.CommonData : DataStorage.PlayerData;
        pData.Set<T>(varName, varValue);
    }

    public static T GetVar<T>(string varName, T defaultValue = default, bool bCommon = false)
    {
        IProfileData pData = bCommon ? DataStorage.CommonData : DataStorage.PlayerData;
        return pData.Get<T>(varName, defaultValue);
    }
    
    public static bool IsVarExist<T>(string varName, bool bCommon = false)
    {
        IProfileData pData = bCommon ? DataStorage.CommonData : DataStorage.PlayerData;
        return pData.HasKey<T>(varName);
    }
    
    public static void DeleteVar<T>(string varName, bool bCommon = false)
    {
        IProfileData pData = bCommon ? DataStorage.CommonData : DataStorage.PlayerData;
        pData.Remove<T>(varName);
    }
}
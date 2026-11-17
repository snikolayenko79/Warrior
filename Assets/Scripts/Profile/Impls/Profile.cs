using UnityEngine;
using System.Threading.Tasks;

public static class Profile
{
    public static class ProfileKeys
    {
        public const string COMMON_FILE_NAME = "CommonProfile";
        public const string PLAYER_FILE_PREFIX = "PlayerProfile_";
        public const string DEFAULT_PLAYER_NAME = "Warrior";
    }
    
    private static readonly IProfileSaver Saver = new JsonProfileSaver();

    private static readonly IProfileDataStorage<ProfileData> DataStorage = new ProfileDataStorage<ProfileData>();

    private static string _currentPlayerId = "Default";

    public static bool IsCommonLoaded { get; private set; }
    public static bool IsPlayerLoaded { get; private set; }

    public static string GetProfileFolder()
    {
        return Saver.ProfileFolder;
    }
    
    public static async Task LoadCommonAsync()
    {
        DataStorage.CommonData = await Saver.LoadAsync<ProfileData>(ProfileKeys.COMMON_FILE_NAME);
        IsCommonLoaded = true;
    }

    public static async Task LoadPlayerAsync(string playerId)
    {
        _currentPlayerId = playerId;
        string fileName = $"{ProfileKeys.PLAYER_FILE_PREFIX}{playerId}";
        
        DataStorage.PlayerData = await Saver.LoadAsync<ProfileData>(fileName);
        IsPlayerLoaded = true;
    }
    
    public static void LoadCommonSync()
    {
        if (IsCommonLoaded)
            return;
        
        DataStorage.CommonData = Saver.LoadSync<ProfileData>(ProfileKeys.COMMON_FILE_NAME);
        IsCommonLoaded = true;
    }

    public static void LoadPlayerSync(string playerId)
    {
        _currentPlayerId = playerId;
        string fileName = $"{ProfileKeys.PLAYER_FILE_PREFIX}{playerId}";
        
        DataStorage.PlayerData = Saver.LoadSync<ProfileData>(fileName);
        IsPlayerLoaded = true;
    }
    
    public static async Task SaveCommonAsync()
    {
        await Saver.SaveAsync(ProfileKeys.COMMON_FILE_NAME, DataStorage.CommonData);
    }

    public static async Task SavePlayerAsync()
    {
        string fileName = $"{ProfileKeys.PLAYER_FILE_PREFIX}{_currentPlayerId}";
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
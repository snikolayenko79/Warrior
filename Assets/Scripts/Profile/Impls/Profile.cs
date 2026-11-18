using UnityEngine;
using System.Threading;
using System.Threading.Tasks;

public static class Profile
{
    public static class ProfileKeys
    {
        public const string COMMON_FILE_NAME = "CommonProfile";
        public const string PLAYER_FILE_PREFIX = "PlayerProfile_";
        public const string DEFAULT_PLAYER_NAME = "Warrior";
        public const int SAVE_DELAY_MS = 1500; // Задержка перед реальной записью на диск (в миллисекундах)
    }
    
    private static readonly IProfileSaver Saver = new JsonProfileSaver();

    private static readonly IProfileDataStorage<ProfileData> DataStorage = new ProfileDataStorage<ProfileData>();

    private static string _currentPlayerId = "Default";
    
    // Токены для отмены и перезапуска таймеров
    private static CancellationTokenSource _playerSaveCts;
    private static CancellationTokenSource _commonSaveCts;
    
    // Флаги «грязных» данных (показывают, есть ли несохраненные изменения)
    private static bool _isPlayerDirty;
    private static bool _isCommonDirty;

    public static bool IsCommonLoaded { get; private set; }
    public static bool IsPlayerLoaded { get; private set; }

    public static string GetProfileFolder()
    {
        return Saver.ProfileFolder;
    }
    
    // --- ЗАГРУЗКА ---
    
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
    
    // --- ОТЛОЖЕННОЕ СОХРАНЕНИЕ (DEBOUNCE) ---

    /// <summary>
    /// Запланировать сохранение профиля игрока через 1.5 сек после последнего изменения.
    /// </summary>
    public static void RequestSavePlayer()
    {
        _isPlayerDirty = true;

        // Отменяем предыдущий таймер, если он еще тикает
        _playerSaveCts?.Cancel();
        _playerSaveCts?.Dispose();
        _playerSaveCts = new CancellationTokenSource();

        _ = DelayedSavePlayerAsync(_playerSaveCts.Token);
    }

    /// <summary>
    /// Запланировать сохранение общих настроек через 1.5 сек после последнего изменения.
    /// </summary>
    public static void RequestSaveCommon()
    {
        _isCommonDirty = true;

        _commonSaveCts?.Cancel();
        _commonSaveCts?.Dispose();
        _commonSaveCts = new CancellationTokenSource();

        _ = DelayedSaveCommonAsync(_commonSaveCts.Token);
    }

    private static async Task DelayedSavePlayerAsync(CancellationToken token)
    {
        try
        {
            await Task.Delay(ProfileKeys.SAVE_DELAY_MS, token);
            if (!token.IsCancellationRequested)
            {
                await SavePlayerAsync();
            }
        }
        catch (TaskCanceledException)
        {
            // Таймер был сброшен новым вызовом RequestSavePlayer — это нормальное поведение
        }
    }

    private static async Task DelayedSaveCommonAsync(CancellationToken token)
    {
        try
        {
            await Task.Delay(ProfileKeys.SAVE_DELAY_MS, token);
            if (!token.IsCancellationRequested)
            {
                await SaveCommonAsync();
            }
        }
        catch (TaskCanceledException)
        {
            // Таймер сброшен
        }
    }

    // --- ПРЯМОЕ (МГНОВЕННОЕ) СОХРАНЕНИЕ ---
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
    
    /// <summary>
    /// Немедленно сохраняет только те профили, в которых есть несохраненные изменения.
    /// Идеально для OnApplicationPause и OnApplicationQuit.
    /// </summary>
    public static async Task FlushPendingSavesAsync()
    {
        Task commonTask = _isCommonDirty ? SaveCommonAsync() : Task.CompletedTask;
        Task playerTask = _isPlayerDirty ? SavePlayerAsync() : Task.CompletedTask;

        await Task.WhenAll(commonTask, playerTask);
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
        
        if (bCommon)
            RequestSaveCommon();
        else
            RequestSavePlayer();
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
        
        if (bCommon)
            RequestSaveCommon();
        else
            RequestSavePlayer();
    }
}
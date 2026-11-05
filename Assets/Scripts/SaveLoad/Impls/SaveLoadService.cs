using UnityEngine;

public class SaveLoadService : ISaveLoadService
{
    // По умолчанию создаем чистый прогресс новой игры
    public PlayerSaveData CurrentProgress { get; set; } = new PlayerSaveData();
}
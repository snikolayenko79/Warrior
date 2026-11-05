using UnityEngine;

// Временный класс-контейнер для хранения данных игрока между сценами
// TODO. Заменить на универсальный данные, DataStorage, хранящий переменные разных типов.
public class PlayerSaveData
{
    public bool IsNewData = true;
    public float SavedHealth = 100f;
    public float MaxHealth = 100f;
    public int Coins = 100;
    public int Experience = 10;
}

public interface ISaveLoadService
{
    // TODO. Заменить на ф-ции получения переменных разных типов.
    PlayerSaveData CurrentProgress { get; set; }
}
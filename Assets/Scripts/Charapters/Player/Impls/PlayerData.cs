using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class PlayerData : MonoBehaviour, IPlayerData, ISavable
{
    [Header("Player init data")]
    [SerializeField]private float maxHealthInit;
    [SerializeField]private int coinsInit;
    [SerializeField]private int experienceInit;
    
    private float _currentHealth;
    private float _currentMaxHealth;
    private int _currentCoins;
    private int _currentExperience;
    
    public float CurrentHealth
    {
        get => _currentHealth;
        set => _currentHealth = Mathf.Clamp(value, 0, _currentMaxHealth);
    }
    
    public float MaxHealth => _currentMaxHealth;

    public int Coins
    {
        get => _currentCoins;
        set => _currentCoins = value;
    }

    public int Experience
    {
        get => _currentExperience;
        set => _currentExperience = value;
    }
    
    [Inject]
    public void Construct(ISaveLoadService saveLoadService)
    {
        PlayerSaveData rawData = saveLoadService.CurrentProgress;

        if (rawData.IsNewData)
        {
            ResetData();
        }
        else
        {
            _currentMaxHealth = rawData.MaxHealth;
            _currentHealth = rawData.SavedHealth;
            _currentCoins = rawData.Coins;
            _currentExperience = rawData.Experience;
        }
    }
    
    public void Save(ISaveLoadService saveService)
    {
        PlayerSaveData rawData = saveService.CurrentProgress;
        
        rawData.MaxHealth = _currentMaxHealth;
        rawData.SavedHealth = _currentHealth;
        rawData.Coins = _currentCoins;
        rawData.Experience = _currentExperience;
        
        rawData.IsNewData = false;
    }
    
    public void ResetData()
    {
        _currentMaxHealth = maxHealthInit;
        _currentHealth = _currentMaxHealth;
        _currentCoins = coinsInit;
        _currentExperience = experienceInit;
    }
}
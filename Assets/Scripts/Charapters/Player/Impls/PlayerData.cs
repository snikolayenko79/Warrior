using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class PlayerData : AttackCharacterData, ICustomerData, ISavable
{
    [SerializeField]private int coinsInit;
    
    private int _currentCoins;

    public int Coins
    {
        get => _currentCoins;
        set
        {
            _currentCoins = value;
            RaiseDataChanged();
        }
    }

    private ISaveLoadService _saveLoadService;
        
    [Inject]
    public void Construct(ISaveLoadService saveLoadService)
    {
        _saveLoadService = saveLoadService;
    }
    
    public override void Initialize()
    {
        PlayerSaveData rawData = _saveLoadService.CurrentProgress;

        if (!rawData.IsNewData)
        {
            Load(_saveLoadService);
        }
        else
        {
            _currentMaxHealth = maxHealthInit;
            _health = _currentMaxHealth;
            _currentCoins = coinsInit;
            _currentExperience = experienceInit;
        }
        
        RaiseDataInitialized();
    }
    
    public void Save(ISaveLoadService saveService)
    {
        PlayerSaveData rawData = saveService.CurrentProgress;
        
        rawData.MaxHealth = MaxHealth;
        rawData.SavedHealth = Health;
        rawData.Coins = Coins;
        rawData.Experience = Experience;
        
        rawData.IsNewData = false;
    }
    
    public void Load(ISaveLoadService saveService)
    {
        PlayerSaveData rawData = saveService.CurrentProgress;
        
        rawData.MaxHealth = MaxHealth;
        rawData.SavedHealth = Health;
        rawData.Coins = Coins;
        rawData.Experience = Experience;
    }
}
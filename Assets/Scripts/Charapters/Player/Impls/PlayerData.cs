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
        set => _currentCoins = value;
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
            MaxHealth = maxHealthInit;
            Health = MaxHealth;
            Coins = coinsInit;
            Experience = experienceInit;
        }
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
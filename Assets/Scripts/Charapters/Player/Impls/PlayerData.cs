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
        bool isNewData = Profile.GetVar<bool>(name + "_isNewData");
        if (!isNewData)
        {
            Load();
        }
        else
        {
            _currentMaxHealth = maxHealthInit;
            _currentHealth = _currentMaxHealth;
            _currentCoins = coinsInit;
            _currentExperience = experienceInit;
        }
        
        RaiseDataInitialized();
    }
    
    public void Save()
    {
        Profile.SetVar<float>(name + "_maxHealth", _currentMaxHealth);
        Profile.SetVar<float>(name + "_health", _currentHealth);
        Profile.SetVar<int>(name + "_coins", _currentCoins);
        Profile.SetVar<int>(name + "_experience", _currentExperience);
        
        Profile.SetVar<bool>(name + "_isNewData", false);
    }
    
    public void Load()
    {
        _currentMaxHealth = Profile.GetVar<float>(name + "_maxHealth");
        _currentHealth = Profile.GetVar<float>(name + "_health");
        _currentCoins = Profile.GetVar<int>(name + "_coins");
        _currentExperience = Profile.GetVar<int>(name + "_experience");
    }
}
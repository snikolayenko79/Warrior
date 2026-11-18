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
    
    public override void OnInitialize()
    {
        Load();
    }

    public override void OnDataChanged()
    {
        Save();
    }

    public void Save()
    {
        Profile.SetVar<float>(Container.Name + "_maxHealth", _currentMaxHealth);
        Profile.SetVar<float>(Container.Name  + "_health", _currentHealth);
        Profile.SetVar<int>(Container.Name  + "_coins", _currentCoins);
        Profile.SetVar<int>(Container.Name  + "_experience", _currentExperience);
        
        Profile.SetVar<bool>(Container.Name  + "_isNewData", false);
    }
    
    public void Load()
    {
        _currentMaxHealth = Profile.GetVar<float>(Container.Name  + "_maxHealth", maxHealthInit);
        _currentHealth = Profile.GetVar<float>(Container.Name  + "_health", _currentMaxHealth);
        _currentCoins = Profile.GetVar<int>(Container.Name  + "_coins", coinsInit);
        _currentExperience = Profile.GetVar<int>(Container.Name  + "_experience", experienceInit);
    }
}
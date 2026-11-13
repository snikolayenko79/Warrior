using System;
using UnityEngine;
using TMPro;
using Zenject;

public class PlayerDataUI : MonoBehaviour
{
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI manaText;
    public TextMeshProUGUI experienceText;
    public TextMeshProUGUI coinsText;

    private PlayerData _playerData;
    private IGameManager _gameManager;

    [Inject]
    public void Construct(IGameManager gameManager)
    {
        _gameManager = gameManager;

        if (_gameManager.PlayerCharacter != null)
        {
            if (_gameManager.PlayerCharacter is Player player)
            {
                if (_playerData != null)
                {
                    _playerData.OnDataInitialized -= OnDataInitialized;
                    _playerData.OnDataChanged -= OnDataChanged;
                }
                
                _playerData = player.CurrentPlayerData;
                
                _playerData.OnDataInitialized += OnDataInitialized;
                _playerData.OnDataChanged += OnDataChanged;
                
                UpdateUI(_playerData);
            }
        }
        
        if (isActiveAndEnabled)
            _gameManager.OnPlayerCreated += OnPlayerCreated;
    }

    public void OnEnable()
    {
        if (_gameManager != null)
            _gameManager.OnPlayerCreated += OnPlayerCreated;
    }
    
    public void OnDisable()
    {
        if (_gameManager != null)
            _gameManager.OnPlayerCreated -= OnPlayerCreated;
    }

    private void OnPlayerCreated(ICharacter playerCharacter)
    {
        if (playerCharacter is Player player)
        {
            if (_playerData != null)
            {
                _playerData.OnDataInitialized -= OnDataInitialized;
                _playerData.OnDataChanged -= OnDataChanged;
            }
            
            _playerData = player.CurrentPlayerData;
            
            _playerData.OnDataInitialized += OnDataInitialized;
            _playerData.OnDataChanged += OnDataChanged;
            
            UpdateUI(_playerData);
        }
    }

    private void OnDataInitialized(IData data)
    {
        UpdateUI(_playerData);
    }
    
    private void OnDataChanged(IData data)
    {
        UpdateUI(_playerData);
    }

    private void UpdateUI(PlayerData playerData)
    {
        if (playerData == null)
            return;
        
        if (healthText)
            healthText.text = playerData.Health.ToString() + "/" + playerData.MaxHealth.ToString();
        //if (manaText)
        //    manaText.text = _playerCharacter.Mana.ToString();
        if (experienceText)
            experienceText.text = playerData.Experience.ToString() + "/100";
        if (healthText)
            coinsText.text = playerData.Coins.ToString();
        
    }
}
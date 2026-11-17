using UnityEngine;
using System;
using System.Threading.Tasks;
using Zenject;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] private GameObject loadingPrefab;
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    private ISceneLoader _sceneLoader;
    private UIManager _uiManager;

    private delegate void OnInitialized();
    
    [Inject]
    public void Construct(ISceneLoader sceneLoader,  UIManager uiManager)
    {
        _sceneLoader = sceneLoader;
        _uiManager = uiManager;
    }
    
    private void Awake()
    {
        BeforeInitialize();
        _ = InitializeAsync(
            AfterInitialize);
    }

    private async Task InitializeAsync(OnInitialized callback)
    {
        try
        {
            await Profile.LoadCommonAsync();
            await Profile.LoadPlayerAsync(Profile.ProfileKeys.DEFAULT_PLAYER_NAME);
            
            await Task.Delay(2000);

            if (this != null)
            {
                callback?.Invoke();
            }
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    private void BeforeInitialize()
    {
        _uiManager.CreateView(loadingPrefab);
    }
    
    private void AfterInitialize()
    {
        _sceneLoader.LoadScene(mainMenuSceneName);
    }
}
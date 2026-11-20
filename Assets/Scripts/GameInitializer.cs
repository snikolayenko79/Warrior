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

    private IUIView _loadingView;

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
            await Profile.SaveAllAsync();
            
            await Task.Delay(1000);

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
        CreateLoadingView();
    }
    
    private void AfterInitialize()
    {
        DestroyLoadingView();
        _sceneLoader.LoadScene(mainMenuSceneName);
    }

    private void CreateLoadingView()
    {
        _loadingView = _uiManager.CreateView(loadingPrefab);
    }
    
    private void DestroyLoadingView()
    {
        if (!_loadingView.IsUnityNull())
            _loadingView.Destroy();
    }
}
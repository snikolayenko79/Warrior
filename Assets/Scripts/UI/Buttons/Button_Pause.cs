using UnityEngine;
using Zenject;

public class Button_Pause : MonoBehaviour
{
    private IUIManager _uiManager;
    private GameObject _pauseMenuPrefab;

    [Inject]
    public void Construct(IUIManager uiManager, [Inject(Id = "PauseMenuPrefab")] GameObject pauseMenuPrefab)
    {
        _uiManager = uiManager;
        _pauseMenuPrefab  = pauseMenuPrefab;
    }

    public void OnClick()
    {
        _uiManager.OpenWindow(_pauseMenuPrefab);
    }
}
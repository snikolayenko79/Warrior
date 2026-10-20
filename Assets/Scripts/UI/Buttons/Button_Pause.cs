using UnityEngine;
using Zenject;

public class Button_Pause : MonoBehaviour
{
    private IUIManager _uiManager;
    private IUIView _pauseMenuPrefab;

    [Inject]
    public void Construct(IUIManager uiManager, [Inject(Id = "PauseMenuPrefab")] IUIView pauseMenuPrefab)
    {
        _uiManager = uiManager;
        _pauseMenuPrefab  = pauseMenuPrefab;
    }

    public void OnClick()
    {
        _uiManager.OpenPopup(_pauseMenuPrefab);
    }
}
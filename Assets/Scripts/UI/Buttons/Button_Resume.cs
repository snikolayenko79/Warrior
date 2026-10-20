using UnityEngine;
using Zenject;

public class Button_Resume : MonoBehaviour
{
    private IUIManager _uiManager;

    [Inject]
    public void Construct(IUIManager uiManager)
    {
        _uiManager = uiManager;
    }

    public void OnClick()
    {
        _uiManager.CloseCurrentPopup();
    }
}
using UnityEngine;
using Zenject;

public class Button_CloseView: MonoBehaviour
{
    private IUIManager _uiManager;

    [Inject]
    public void Construct(IUIManager uiManager)
    {
        _uiManager  = uiManager;
    }
    
    public void OnClick()
    {
        _uiManager.CloseActiveView();
    }
}

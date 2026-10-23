using UnityEngine;
using Zenject;

public class Button_DialogueNext : MonoBehaviour
{
    private IDialogManager _dialogManager;

    [Inject]
    public void Construct(IDialogManager dialogManager)
    {
        _dialogManager  = dialogManager;
        
    }
    
    public void OnClick()
    {
        _dialogManager.DialogueNext();
    }
}
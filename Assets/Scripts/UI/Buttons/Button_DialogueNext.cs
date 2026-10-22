using UnityEngine;
using Zenject;

public class Button_DialogueNext : MonoBehaviour
{
    private IDialogManager _dialogManager;

    [Inject]
    public void Construct(IDialogManager dialogManager)
    {
        Debug.Log(dialogManager.ToString());
        _dialogManager  = dialogManager;
        
    }
    
    public void OnClick()
    {
        Debug.Log(_dialogManager.ToString());
        _dialogManager.DialogueNext();
    }
}
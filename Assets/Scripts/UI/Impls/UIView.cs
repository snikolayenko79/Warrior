using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UIView : MonoBehaviour, IUIView
{
    public bool IsViewModal = false;
    public bool IsViewPauseTime = false;
    
    public bool IsModal => IsViewModal;
    public bool PauseTime => IsViewPauseTime;

    private CanvasGroup _canvasGroup;
    
    public void SetInteractable(bool isInteractable)
    {
        if  (_canvasGroup == null)
            _canvasGroup = GetComponent<CanvasGroup>();
        
        _canvasGroup.interactable = isInteractable;
        _canvasGroup.blocksRaycasts = isInteractable;
        _canvasGroup.alpha = isInteractable ? 1 : 0.5f;
    }
    
    public GameObject GameObject => this.gameObject;
}

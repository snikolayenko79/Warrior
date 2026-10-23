using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

[RequireComponent(typeof(CanvasGroup))]
public abstract class UIView : MonoBehaviour, IUIView
{
    public bool isViewModal = false;
    public bool isViewPauseTime = false;
    
    public bool IsModal => isViewModal;
    public bool PauseTime => isViewPauseTime;

    private CanvasGroup _canvasGroup;
    
    public void SetInteractable(bool isInteractable)
    {
        if  (!_canvasGroup)
            TryGetComponent<CanvasGroup>(out _canvasGroup);
        
        _canvasGroup.interactable = isInteractable;
        _canvasGroup.blocksRaycasts = isInteractable;
        _canvasGroup.alpha = isInteractable ? 1 : 0.5f;
    }

    public abstract void OnCreate(params object[] args);
    
    public void Destroy()
    {
        UnityEngine.Object.Destroy(gameObject);
    }
}

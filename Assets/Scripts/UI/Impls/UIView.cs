using UnityEngine;
using Zenject;

[RequireComponent(typeof(CanvasGroup))]
public abstract class UIView : MonoBehaviour, IUIView
{
    public bool IsViewModal = false;
    public bool IsViewPauseTime = false;
    
    public bool IsModal => IsViewModal;
    public bool PauseTime => IsViewPauseTime;

    private CanvasGroup _canvasGroup;

    private DiContainer _container;

    [Inject]
    public void Construct(DiContainer container)
    {
        _container  = container;
        
    }
    
    public void SetInteractable(bool isInteractable)
    {
        if  (_canvasGroup == null)
            _canvasGroup = GetComponent<CanvasGroup>();
        
        _canvasGroup.interactable = isInteractable;
        _canvasGroup.blocksRaycasts = isInteractable;
        _canvasGroup.alpha = isInteractable ? 1 : 0.5f;
    }
    
    public GameObject GameObject => this.gameObject;

    public IUIView Create(GameObject prefab, params object[] args)
    {
        Debug.Log(_container.ToString());
        IUIView view = _container.InstantiatePrefabForComponent<IUIView>(prefab);
        if  (view != null)
            view.Init(args);
        return view;
    }

    public abstract void Init(params object[] args);
    
    public void Destroy()
    {
        UnityEngine.Object.Destroy(gameObject);
    }
}

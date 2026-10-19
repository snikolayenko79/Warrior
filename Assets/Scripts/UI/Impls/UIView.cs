using UnityEngine;

public class UIView : MonoBehaviour, IUIView
{
    public void SetInteractable(bool isInteractable)
    {
        
    }
    
    public GameObject GameObject => this.gameObject;
}

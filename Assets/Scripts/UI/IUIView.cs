using UnityEngine;

public interface IUIView
{
    //bool IsModal {get; set;}
    void SetInteractable(bool isInteractable);
    GameObject GameObject { get; }
}

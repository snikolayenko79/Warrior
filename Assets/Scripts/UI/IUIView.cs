using UnityEngine;

public interface IUIView
{
    bool IsModal { get; }
    bool PauseTime { get; }
    void SetInteractable(bool isInteractable);
    GameObject GameObject { get; }
}

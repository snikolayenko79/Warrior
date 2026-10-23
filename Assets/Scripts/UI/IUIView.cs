using UnityEngine;

public interface IUIView
{
    bool IsModal { get; }
    bool PauseTime { get; }
    void SetInteractable(bool isInteractable);
    void OnCreate(params object[] args);
    void Destroy();
}

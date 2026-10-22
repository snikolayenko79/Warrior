using UnityEngine;

public interface IUIView
{
    bool IsModal { get; }
    bool PauseTime { get; }
    void SetInteractable(bool isInteractable);
    void Init(params object[] args);
    IUIView Create(GameObject prefab, params object[] args);
    void Destroy();
}

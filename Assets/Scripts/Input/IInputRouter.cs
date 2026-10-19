using UnityEngine;

public interface IInputRouter
{
    IInputEventSource InputSource { get; }
    
    void Register(IInputListener listener);
    void Unregister(IInputListener listener);
}
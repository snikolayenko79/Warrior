using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class InputRouter : IInputRouter
{
    public IInputEventSource InputSource => MyInputSource;
    private IInputEventSource MyInputSource;
    
    public void Initialize(IInputEventSource inputSource)
    {
        MyInputSource = inputSource;
        
        MyInputSource.OnInputAction += OnInputAction;
    }
    
    private Dictionary<string, List<IInputListener>> InputMap = new  Dictionary<string, List<IInputListener>>();
    
    public void Register(IInputListener listener)
    {
        foreach (var actionName in listener.ActionsName)
        {
            if (!InputMap.ContainsKey(actionName))
                InputMap[actionName] = new List<IInputListener>();
        
            if (!InputMap[actionName].Contains(listener))
                InputMap[actionName].Add(listener);
        }
    }
    
    public void Unregister(IInputListener listener)
    {
        foreach (var actionName in listener.ActionsName)
        {
            if (InputMap.ContainsKey(actionName))
                InputMap[actionName].Remove(listener);

            if (InputMap.TryGetValue(actionName, out List<IInputListener> listeners))
            {
                listeners.Remove(listener);
                
                // ОПТИМИЗАЦИЯ: Если для этого действия больше нет слушателей,
                // полностью удаляем ключ из словаря, освобождая память.
                if (listeners.Count == 0)
                {
                    InputMap.Remove(actionName);
                }
            }
        }
    }

    private void OnInputAction(string actionName, InputContext inputContext)
    {
        if (InputMap.TryGetValue(actionName, out List<IInputListener> listeners))
        {
            foreach (var listener in listeners)
            {
                listener.HandleInput(actionName, inputContext);
            }
        }
    }
}

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
        Debug.Log($"Registering {listener.ActionsName}");
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
        }
    }

    private void OnInputAction(string actionNane, float value)
    {
        if (InputMap.ContainsKey(actionNane))
        {
            foreach (var listener in InputMap[actionNane])
            {
                listener.HandleInput(actionNane, value);
            }
        }
    }
}

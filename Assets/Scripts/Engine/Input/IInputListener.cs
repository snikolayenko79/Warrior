using UnityEngine;
using System.Collections.Generic;

public interface IInputListener
{
    List<string> ActionsName { get; }
    void HandleInput(string logicalActionName, InputContext inputContextcontext);
    delegate void InputActionHandler(string actionName, InputContext context);
    event  InputActionHandler OnInputAction;
}

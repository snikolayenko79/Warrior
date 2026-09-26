using UnityEngine;
using System.Collections.Generic;

public interface IInputListener
{
    List<string> ActionsName { get; }
    void HandleInput(string actionName, InputContext inputContextcontext);
}

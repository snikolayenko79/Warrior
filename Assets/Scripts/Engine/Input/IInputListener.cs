using UnityEngine;
using System.Collections.Generic;

public interface IInputListener
{
    List<string> ActionsName { get; }
    void HandleInput(string logicalActionName, InputContext inputContextcontext);
}

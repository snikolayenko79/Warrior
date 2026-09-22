using UnityEngine;
using System;

public interface IInputEventSource
{
    event Action<string, float> OnInputAction;
}
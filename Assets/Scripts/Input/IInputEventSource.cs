using UnityEngine;
using System;

public struct InputContext
{
    // Значение для кнопок, курков и одномерных осей
    public float FloatValue { get; private set; }
    
    // Значение для двумерных осей (WASD, стики геймпада)
    public Vector2 VectorValue { get; private set; }

    // Удобное свойство-помощник, чтобы быстро проверять, нажата ли кнопка
    //public bool IsPressed => Mathf.Abs(FloatValue) > 0.01f || VectorValue.sqrMagnitude > 0.01f;

    // Конструктор для одномерных данных
    public InputContext(float floatValue)
    {
        FloatValue = floatValue;
        VectorValue = Vector2.zero;
    }

    // Конструктор для двумерных данных
    public InputContext(Vector2 vectorValue)
    {
        FloatValue = vectorValue.magnitude; // Длина вектора как float-значение
        VectorValue = vectorValue;
    }
}

public interface IInputEventSource
{
    event Action<string, InputContext> OnInputAction;
}
using System;
using UnityEngine;
using UnityEngine.InputSystem; // Обязательный namespace для новой системы

public class NewInputSystemSource : MonoBehaviour, IInputEventSource, GameControls.IGameplayActions
{
    public event Action<string, float> OnInputAction;
    
    private GameControls controls;

    private float LastRotateDirection = 0;

    private void Awake()
    {
        // Инициализируем сгенерированный Unity класс управления
        controls = new GameControls();
        
        // Регистрируем этот класс как обработчик событий карты Gameplay
        controls.Gameplay.SetCallbacks(this);
    }

    private void OnEnable()
    {
        // Включаем карту действий при активации объекта
        controls.Gameplay.Enable();
    }

    private void OnDisable()
    {
        // Выключаем карту действий, чтобы не тратить ресурсы
        controls.Gameplay.Disable();
    }

    public void OnRotate(InputAction.CallbackContext context)
    {
        // Считываем текущее значение направления вращения (-1, 0, 1)
        float currentDirection = context.ReadValue<float>();

        // Передаем событие дальше по цепочке в Paddle только при изменении значения
        if (Mathf.Approximately(currentDirection, LastRotateDirection) == false)
        {
            LastRotateDirection = currentDirection;
            OnInputAction?.Invoke(context.action.name, currentDirection);
        }
    }
    
    public void OnMove(InputAction.CallbackContext context)
    {
        OnInputAction?.Invoke(context.action.name, 0);
    }
    
    public void OnAttack(InputAction.CallbackContext context)
    {
        OnInputAction?.Invoke(context.action.name, 0);
    }
    
    public void OnSpell(InputAction.CallbackContext context)
    {
        OnInputAction?.Invoke(context.action.name, 0);
    }
    
    public void OnClick(InputAction.CallbackContext context)
    {
        OnInputAction?.Invoke(context.action.name, 0);
    }
}
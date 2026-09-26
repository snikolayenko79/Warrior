using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem; // Обязательный namespace для новой системы

public class NewInputEventSource_CustomActions : MonoBehaviour, IInputEventSource, GameControls.IGameplayActions
{
    public event Action<string, InputContext> OnInputAction;
    
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
            OnInputAction?.Invoke(context.action.name, new InputContext(currentDirection));
        }
    }
    
    public void OnMove(InputAction.CallbackContext context)
    {
        // Предполагаем, что может быть либо флоат, либо вектор.
        OnInputAction?.Invoke(context.action.name, context.valueType == typeof(Vector2) ? new InputContext(context.ReadValue<Vector2>()) : new InputContext(context.ReadValue<float>()));
    }
    
    public void OnAttack(InputAction.CallbackContext context)
    {
        // Для обычных кнопок передаем 1 (нажато) или 0 (отпущено)
        float value = context.performed ? 1f : 0f;
        OnInputAction?.Invoke(context.action.name, new InputContext(value));
    }
    
    public void OnSpell(InputAction.CallbackContext context)
    {
        // Для обычных кнопок передаем 1 (нажато) или 0 (отпущено)
        float value = context.performed ? 1f : 0f;
        OnInputAction?.Invoke(context.action.name, new InputContext(value));
    }
    
    public void OnClick(InputAction.CallbackContext context)
    {
        // Для обычных кнопок передаем 1 (нажато) или 0 (отпущено)
        float value = context.performed ? 1f : 0f;
        OnInputAction?.Invoke(context.action.name, new InputContext(value));
    }
}
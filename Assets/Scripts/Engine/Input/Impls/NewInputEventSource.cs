using UnityEngine;
using UnityEngine.InputSystem;
using System;

[RequireComponent(typeof(PlayerInput))]
public class NewInputEventSource : MonoBehaviour, IInputEventSource
{
    public event Action<string, InputContext> OnInputAction;

    private PlayerInput _playerInput;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _playerInput.notificationBehavior = PlayerNotifications.InvokeCSharpEvents;
    }

    private void OnEnable()
    {
        if (_playerInput != null)
        {
            // Подписываемся на ОДНО универсальное событие для ВСЕХ экшенов
            _playerInput.onActionTriggered += OnAnyActionTriggered;
        }
    }

    private void OnDisable()
    {
        if (_playerInput != null)
        {
            _playerInput.onActionTriggered -= OnAnyActionTriggered;
        }
    }

    // Этот метод вызывается автоматически при ЛЮБОМ действии (нажатие, движение, отпускание)
    private void OnAnyActionTriggered(InputAction.CallbackContext context)
    {
        // Получаем техническое имя экшена напрямую из настроек Unity (например, "Move", "Attack")
        string hardwareActionName = context.action.name;
        
        Debug.Log(hardwareActionName);

        // Определяем тип данных экшена динамически
        if (context.valueType == typeof(Vector2))
        {
            // Если это Vector2 (оси, стики, WASD)
            Vector2 vectorVal = context.ReadValue<Vector2>();
            
            // Генерируем событие только при изменении состояния (чтобы не спамить нулями)
            if (context.performed || context.canceled)
            {
                OnInputAction?.Invoke(hardwareActionName, new InputContext(vectorVal));
            }
        }
        else if (context.valueType == typeof(float))
        {
            // Если это float (одиночная ось, триггер геймпада или кнопка-модификатор)
            float floatVal = context.ReadValue<float>();
            
            if (context.performed || context.canceled)
            {
                OnInputAction?.Invoke(hardwareActionName, new InputContext(floatVal));
            }
        }
        else
        {
            // Если это простая кнопка (Button/Trigger без явного float типа)
            // performed = кнопка зажата (1.0), canceled = кнопку отпустили (0.0)
            if (context.performed)
            {
                OnInputAction?.Invoke(hardwareActionName, new InputContext(1f));
            }
            else if (context.canceled)
            {
                OnInputAction?.Invoke(hardwareActionName, new InputContext(0f));
            }
        }
    }
}

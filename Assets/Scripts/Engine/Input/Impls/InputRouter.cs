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

        LoadMapping();
        
        MyInputSource.OnInputAction += OnInputAction;
    }
    
    // Карта: ЛогическоеИмяДействия -> Список слушателей из игры
    private Dictionary<string, List<IInputListener>> InputMap = new  Dictionary<string, List<IInputListener>>();
    
    // Словарь соответствий (ТехническоеИмяОтВвода -> ЛогическоеИмяВИгре)
    private Dictionary<string, string> _actionMapping;
    
    public void Register(IInputListener listener)
    {
        foreach (var logicalActionName in listener.ActionsName)
        {
            if (!InputMap.ContainsKey(logicalActionName))
                InputMap[logicalActionName] = new List<IInputListener>();
        
            if (!InputMap[logicalActionName].Contains(listener))
                InputMap[logicalActionName].Add(listener);
        }
    }
    
    public void Unregister(IInputListener listener)
    {
        foreach (var logicalActionName in listener.ActionsName)
        {
            if (InputMap.ContainsKey(logicalActionName))
                InputMap[logicalActionName].Remove(listener);

            if (InputMap.TryGetValue(logicalActionName, out List<IInputListener> listeners))
            {
                listeners.Remove(listener);
                
                // ОПТИМИЗАЦИЯ: Если для этого действия больше нет слушателей,
                // полностью удаляем ключ из словаря, освобождая память.
                if (listeners.Count == 0)
                {
                    InputMap.Remove(logicalActionName);
                }
            }
        }
    }

    private void OnInputAction(string hardwareActionName, InputContext inputContext)
    {
        // 1. Пытаемся перевести техническое имя в логическое
        if (_actionMapping.TryGetValue(hardwareActionName, out string logicalActionName))
        {
            // 2. Если перевод найден, ищем игровых слушателей, завязаных на это логическое имя
            if (InputMap.TryGetValue(logicalActionName, out List<IInputListener> listeners))
            {
                // // Чтобы не сломался цикл, если в процессе список изменится. Но, могут вылезти неявные баги, когда какой-то подписчик не сработает.
                // for (int i = listeners.Count - 1; i >= 0; i--)
                // {
                //     // Передаем слушателю именно ЛОГИЧЕСКОЕ имя, которое он ждет
                //     listeners[i].HandleInput(logicalActionName, inputContext);
                // }

                // Может слоаматься при смене списка в процессе. Но, сразу будет эксепшн.
                foreach (var listener in  listeners)
                {
                    // Передаем слушателю именно ЛОГИЧЕСКОЕ имя, которое он ждет
                    listener.HandleInput(logicalActionName, inputContext);
                }
            }
        }
    }

    private void LoadMapping()
    {
        // TODO. Загрузка из json.
        _actionMapping = new Dictionary<string, string>()
        {
            { "Rotate", "Rotate" },
            { "Move", "Move" },
            { "Attack", "Attack" },
            { "Spell", "Spell" }
        };
    }
}

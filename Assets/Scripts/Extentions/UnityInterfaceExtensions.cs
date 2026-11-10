using UnityEngine;

public static class UnityInterfaceExtensions
{
    /// <summary>
    /// Безопасно проверяет, уничтожен ли лежащий за интерфейсом Unity-объект.
    /// </summary>
    public static bool IsUnityNull(this object interfaceTarget)
    {
        // 1. Проверяем стандартный C# null
        if (interfaceTarget == null) return true;

        // 2. Приводим к базовому объекту Unity (закроет MonoBehaviour, Component, ScriptableObject и т.д.)
        if (interfaceTarget is UnityEngine.Object unityObject)
        {
            // Используем перегруженный оператор Unity для проверки уничтожения на C++ стороне
            return unityObject == null; 
        }

        // 3. Если это чистый C# класс, и он прошел первую проверку — значит он жив
        return false;
    }
}
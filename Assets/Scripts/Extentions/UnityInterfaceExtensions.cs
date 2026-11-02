using UnityEngine;

public static class UnityInterfaceExtensions
{
    /// <summary>
    /// Безопасно проверяет, уничтожен ли лежащий за интерфейсом Unity-объект.
    /// </summary>
    public static bool IsUnityNull(this object interfaceTarget)
    {
        if (interfaceTarget == null) return true;

        // Если за интерфейсом скрывается MonoBehaviour, проверяем его через логику Unity
        if (interfaceTarget is MonoBehaviour monoBehaviour)
        {
            return monoBehaviour == null; 
        }

        return false;
    }
}
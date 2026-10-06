using UnityEngine;

public class FollowTargetPath : MonoBehaviour, INavigationPath
{
    public Player target;

    public Vector3? GetCurrentTarget()
    {
        // Если игрок погиб или удален, навигатор возвращает null
        if (target == null || target.IsDead)
            return null;

        // Постоянно возвращаем свежие координаты игрока
        return target.Position;
    }

    public void NextTarget()
    {
        // Для преследования этот метод пустой, так как цель "живая" 
        // и мы никогда не "заканчиваем" маршрут, пока цель не сменится глобально.
    }
}
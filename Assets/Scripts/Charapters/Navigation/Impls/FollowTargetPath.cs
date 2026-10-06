using UnityEngine;

public class FollowTargetPath : INavigationPath
{
    private readonly IAttackable _target;

    public FollowTargetPath(IAttackable target)
    {
        _target = target;
    }

    public Vector3? GetCurrentTarget()
    {
        // Если игрок погиб или удален, навигатор возвращает null
        if (_target == null || _target.IsDead)
            return null;

        // Постоянно возвращаем свежие координаты игрока
        return _target.Position;
    }

    public void NextTarget()
    {
        // Для преследования этот метод пустой, так как цель "живая" 
        // и мы никогда не "заканчиваем" маршрут, пока цель не сменится глобально.
    }
}
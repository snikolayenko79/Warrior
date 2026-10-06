using UnityEngine;

public interface INavigationPath
{
    // Возвращает точку, куда двигаться в данный момент. 
    // Если возвращает null — персонаж должен стоять на месте.
    Vector3? GetCurrentTarget();

    // Вызывается контроллером, когда персонаж физически дошел до текущей точки.
    void NextTarget();
}
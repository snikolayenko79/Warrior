using UnityEngine;
using System.Collections.Generic;

public class PatrolPath : MonoBehaviour, INavigationPath
{
    [Header("Настройки маршрута")]
    [SerializeField] private List<Transform> waypoints = new List<Transform>();
    [SerializeField] private float waitTime = 2f;
    
    private int _currentWaypointIndex = 0;
    private float _waitTimer = 0f;
    private bool _isWaiting = false;

    // Метод интерфейса INavigationPath
    public Vector3? GetCurrentTarget()
    {
        if (waypoints == null || waypoints.Count == 0)
            return null;

        if (_isWaiting)
        {
            _waitTimer += Time.deltaTime;
            if (_waitTimer >= waitTime)
            {
                _isWaiting = false;
                _currentWaypointIndex = (_currentWaypointIndex + 1) % waypoints.Count;
            }
            else
            {
                return null; // Ждем на точке, контроллер остановит объект
            }
        }

        Transform currentWp = waypoints[_currentWaypointIndex];
        return currentWp != null ? currentWp.position : null;
    }

    // Метод интерфейса INavigationPath
    public void NextTarget()
    {
        _isWaiting = true;
        _waitTimer = 0f;
    }
}
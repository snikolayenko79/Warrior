using UnityEngine;
using System.Collections.Generic;

public class SpecifiedPath : MonoBehaviour, INavigationPath
{
    [Header("Настройки маршрута")]
    [SerializeField] private List<Transform> waypoints = new List<Transform>();
    [SerializeField] private float waitTime = 2f;
    [SerializeField] private bool looped = false;
    
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
                _currentWaypointIndex += 1;
                if (_currentWaypointIndex >= waypoints.Count && looped)
                    _currentWaypointIndex = 0;
            }
            else
            {
                return null; // Ждем на точке, контроллер остановит объект
            }
        }
        
        if (_currentWaypointIndex >= waypoints.Count) // прошли весь маршрут, он не зациклен
            return null;
        
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
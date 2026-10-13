using UnityEngine;
using System.Collections.Generic;

public class SpecifiedPath : MonoBehaviour, INavigationPath
{
    [Header("Настройки маршрута")]
    [SerializeField] private List<Transform> waypoints = new List<Transform>();
    [SerializeField] private bool looped = false;
    
    private int _currentWaypointIndex = 0;
    
    // Метод интерфейса INavigationPath
    public Vector3? GetCurrentTarget()
    {
        if (waypoints == null || waypoints.Count == 0)
            return null;
        
        if (_currentWaypointIndex >= waypoints.Count) // прошли весь маршрут, он не зациклен
            return null;
        
        Transform currentWp = waypoints[_currentWaypointIndex];
        return currentWp != null ? currentWp.position : null;
    }

    // Метод интерфейса INavigationPath
    public void NextTarget()
    {
        _currentWaypointIndex += 1;
        if (_currentWaypointIndex >= waypoints.Count && looped)
            _currentWaypointIndex = 0;
    }
}
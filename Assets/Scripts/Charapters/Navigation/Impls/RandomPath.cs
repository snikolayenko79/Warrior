using UnityEngine;

public class RandomPath: INavigationPath
{
    private float limit = 2.0f;
    private Vector3 _startPosition;
    private Vector3 _targetPosition = Vector3.zero;

    public RandomPath(IMovable movable)
    {
        _startPosition = movable.Position;
    }
    
    public Vector3? GetCurrentTarget()
    {
        if (_targetPosition == Vector3.zero)
        {
            _targetPosition = _startPosition;
            _targetPosition.x += Random.Range(-limit, limit);
            _targetPosition.z += Random.Range(-limit, limit);
        }
        
        return _targetPosition;
    }

    public void NextTarget()
    {
        _targetPosition = Vector3.zero;
    }
}
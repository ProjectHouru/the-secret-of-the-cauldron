using UnityEngine;

public class PatrolZone: MonoBehaviour
{
    [SerializeField] private Transform[] _points;

    private int _currentIndex = 0;

    public Vector3 GetTarget()
    {
        return _points.Length > 0 ? _points[_currentIndex].position : Vector3.zero;
    }
    
    public void Next()
    {
        if (_points.Length > 0)
        {
            _currentIndex++;
        
            _currentIndex %= _points.Length;
        }
    }
}
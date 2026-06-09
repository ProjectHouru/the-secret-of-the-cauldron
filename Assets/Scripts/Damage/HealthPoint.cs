using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Events;

public class HealthPoint : MonoBehaviour
{
    [SerializeField] private int _maxValue = 1;
    
    private int _currentValue;
    
    public int MaxValue => _maxValue;
    public int CurrentValue => _currentValue;
    public bool IsAlive => _currentValue > 0;

    private event UnityAction<int> _onChangeValue;
    public event UnityAction<int> OnChangeValue
    {
        add { _onChangeValue -= value; _onChangeValue += value; }
        remove => _onChangeValue -= value;
    }

    private event UnityAction _onIncreaseValue;
    public event UnityAction OnIncreaseValue
    {
        add { _onIncreaseValue -= value; _onIncreaseValue += value; }
        remove => _onIncreaseValue -= value;
    }

    private event UnityAction _onDecreaseValue;
    public event UnityAction OnDecreaseValue
    {
        add { _onDecreaseValue -= value; _onDecreaseValue += value; }
        remove => _onDecreaseValue -= value;
    }

    private void Awake()
    {
        ChangeValue(_maxValue);
    }

    public void Increment(int modifier)
    {
        ChangeValue(_currentValue + Mathf.Abs(modifier)); 
    }

    public void Decrement(int modifier)
    {
        ChangeValue(_currentValue - Mathf.Abs(modifier));
    }

    public void Dead()
    {
        ChangeValue(0);
    }

    private void ChangeValue(int newValue)
    {
        var oldValue = _currentValue;

        _currentValue = Mathf.Clamp(newValue, 0, _maxValue);

        if (_currentValue > oldValue)
        {
            _onIncreaseValue?.Invoke();
        }
        else if (_currentValue < oldValue)
        {
            _onDecreaseValue?.Invoke();
        }

        _onChangeValue?.Invoke(newValue);
    }
}
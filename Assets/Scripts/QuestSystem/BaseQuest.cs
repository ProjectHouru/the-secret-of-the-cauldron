using UnityEngine;
using UnityEngine.Events;

public abstract class BaseQuest: MonoBehaviour
{
    protected bool _isCompleted;
    protected bool _isStarted;
    protected bool _isPaused;
    
    public abstract QuestId Id { get;}
    public abstract string Description { get;}
 
    private event UnityAction _onStart;
    public event UnityAction OnStart
    {
        add { _onStart -= value; _onStart += value; }
        remove => _onStart -= value;
    }
    
    private event UnityAction _onCompleted;
    public event UnityAction OnCompleted
    {
        add { _onCompleted -= value; _onCompleted += value; }
        remove => _onCompleted -= value;
    }
    
    private event UnityAction _onUpdated;
    public event UnityAction OnUpdated
    {
        add { _onUpdated -= value; _onUpdated += value; }
        remove => _onUpdated -= value;
    }

    public void Init()
    {
        _onStart = null;
        _onCompleted = null;
        _onUpdated = null;
        
        _isCompleted = false;
        _isStarted = false;
        _isPaused = false;
    }
    
    public void StartQuest()
    {
        if (!_isStarted)
        {
            _isCompleted = false;
            _isStarted = true;
            _isPaused = false;

           DoAfterStart();
           
           UpdateQuest();
           
           _onStart?.Invoke();
        }
    }

    public void PauseQuest()
    {
        _isPaused = true;

        UpdateQuest();
    }

    protected void UpdateQuest()
    {
        if (_isStarted && !_isCompleted)
        {
            _onUpdated?.Invoke();
        }
    }
    
    protected void CompleteQuest()
    {
        if (_isStarted && !_isCompleted)
        {
            _isCompleted = true;
            
            DoAfterComplete();
            
            UpdateQuest();
            
            _onCompleted?.Invoke();

            // Обнуляем обработчики
            _onStart = null;
            _onUpdated = null;
            _onCompleted = null;
        }
    }
    
    public virtual void TriggerQuest()
    {
    }

    protected virtual void DoAfterStart()
    {
    }
    
    protected virtual void DoAfterComplete()
    {
    }
}
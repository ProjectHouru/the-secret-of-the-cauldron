using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class PlayerDetectZone : MonoBehaviour
{
    [SerializeField] private BaseUsableObjectZone _usableObjectZone;
    
    private PlayerController _playerController;

    private UnityAction _onPlayerEnter;
    private UnityAction _onPlayerLeave;
    
    public event UnityAction OnPlayerEnter
    {
        add
        {
            _onPlayerEnter -= value;
            _onPlayerEnter += value;
        }
        remove => _onPlayerEnter -= value;
    }
    
    public event UnityAction OnPlayerLeave
    {
        add
        {
            _onPlayerLeave -= value;
            _onPlayerLeave += value;
        }
        remove => _onPlayerLeave -= value;
    }
    
    public bool TryGetPlayer(out PlayerController playerController)
    {
        playerController = _playerController;
        
        return _playerController;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.TryGetComponent<PlayerController>(out var playerController))
        {
            _playerController = playerController;
            
            if (_usableObjectZone != null)
            {
                _playerController.AddUsableObjectZone(_usableObjectZone);
                _usableObjectZone.Detect(_playerController);
            }
            
            _onPlayerEnter?.Invoke();
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.TryGetComponent<PlayerController>(out var playerController))
        {
            _playerController = playerController;
            
            if (_usableObjectZone != null)
            {
                _playerController.AddUsableObjectZone(_usableObjectZone);
                _usableObjectZone.StayDetect(_playerController);
            }
        }
    }
    
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.TryGetComponent<PlayerController>(out _))
        {
            if (_usableObjectZone != null)
            {
                _playerController.RemoveUsableObjectZone(_usableObjectZone);
                _usableObjectZone.Miss();
            }
            
            _playerController = null;
            
            _onPlayerLeave?.Invoke();
        }
    }
}
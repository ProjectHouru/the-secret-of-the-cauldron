using UnityEngine;

public class SwampZone : MonoBehaviour
{
    [SerializeField] private float _maxSpeed = 0.5f;

    private float _playerDefaultSpeed = -1f;
    
    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.TryGetComponent<PlayerController>(out var playerController))
        {
            var speed = playerController.GetComponent<RunAction>().GetSpeed();

            if (speed != _maxSpeed)
            {
                Debug.Log("Enter Slow Zone " + speed);
                
                _playerDefaultSpeed = speed;
                playerController.GetComponent<RunAction>().SetSpeed(_maxSpeed);
            }
        }
    }
    
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.TryGetComponent<PlayerController>(out var playerController) && _playerDefaultSpeed > -1f)
        {
            Debug.Log("Exit Slow Zone");
            
            playerController.GetComponent<RunAction>().SetSpeed(_playerDefaultSpeed);

            _playerDefaultSpeed = -1f;
        }
    }
}
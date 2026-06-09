using UnityEngine;

[DefaultExecutionOrder(100500)]
public class ContactCheck : MonoBehaviour
{
    private int _leftWallsContactCount;
    private int _rightWallsContactCount;
    private int _groundContactCount;
    
    public bool IsGrounded => _groundContactCount > 0;
    public bool IsLeftBorder => _leftWallsContactCount > 0;
    public bool IsRightBorder => _rightWallsContactCount > 0;

    private void FixedUpdate()
    {
        Clean();
    }

    private void Clean()
    {
        _leftWallsContactCount = 0;
        _rightWallsContactCount = 0;
        _groundContactCount = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EvaluateCollision(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        EvaluateCollision(collision);
    }

    private void EvaluateCollision(Collision2D collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            var normal = collision.GetContact(i).normal;
            
            var downDotProduct = Vector3.Dot(normal, Vector2.up);
            var leftDotProduct = Vector3.Dot(normal, Vector2.right);
            var rightDotProduct = Vector3.Dot(normal, Vector2.left);

            _leftWallsContactCount = leftDotProduct > 0.75 ? _leftWallsContactCount + 1 : _leftWallsContactCount;
            _rightWallsContactCount = rightDotProduct > 0.75 ? _rightWallsContactCount + 1 : _rightWallsContactCount;
            _groundContactCount = downDotProduct > 0.75 ? _groundContactCount + 1 : _groundContactCount;
        }
    }
}

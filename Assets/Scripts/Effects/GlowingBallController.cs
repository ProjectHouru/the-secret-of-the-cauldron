using UnityEngine;

public class GlowingBallController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float moveDistance = 0.5f;
    [SerializeField] private float scaleChangeSpeed = 0.1f;
    [SerializeField] private float minSize = 0.05f;
    [SerializeField] private float maxSize = 0.02f;

    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.localPosition;
    }

    void FixedUpdate()
    {
        MoveBall();
        ScaleBall();
    }

    void MoveBall()
    {
        float newX = initialPosition.x + Mathf.Sin(Time.time * moveSpeed) * moveDistance;
        transform.localPosition = new Vector3(newX, transform.localPosition.y, transform.localPosition.z);
    }

    void ScaleBall()
    {
        float scale = Mathf.Lerp(minSize, maxSize, Mathf.PingPong(Time.time * scaleChangeSpeed, 1));
        transform.localScale = new Vector3(scale, scale, scale);
    }
}

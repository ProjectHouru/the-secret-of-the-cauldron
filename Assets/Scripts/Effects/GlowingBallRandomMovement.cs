using UnityEngine;

public class GlowingBallRandomMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float moveDistance = 0.5f;
    [SerializeField] private float scaleChangeSpeed = 0.1f;
    [SerializeField] private float minSize = 0.05f;
    [SerializeField] private float maxSize = 0.2f;
    [SerializeField] private float rotationSpeed = 10f;

    private Vector3 initialPosition;
    private Vector2 randomOffset;

    void Start()
    {
        initialPosition = transform.localPosition;
        randomOffset = new Vector2(Random.Range(0f, 100f), Random.Range(0f, 100f));
    }

    void FixedUpdate()
    {
        MoveBall();
        ScaleBall();
        RotateBall();
    }

    void MoveBall()
    {
        float offsetX = Mathf.PerlinNoise(Time.time * moveSpeed + randomOffset.x, randomOffset.y) * 2 - 1;
        float offsetY = Mathf.PerlinNoise(randomOffset.x, Time.time * moveSpeed + randomOffset.y) * 2 - 1;

        Vector3 randomPosition = new Vector3(offsetX, offsetY, 0) * moveDistance;
        transform.localPosition = initialPosition + randomPosition; 
    }

    void ScaleBall()
    {
        float scale = Mathf.Lerp(minSize, maxSize, Mathf.PingPong(Time.time * scaleChangeSpeed, 1));
        transform.localScale = new Vector3(scale, scale, scale);
    }

    void RotateBall()
    {
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }
}

using UnityEngine;

public class WindController2D : MonoBehaviour
{
    private ParticleSystem rainSystem;
    private ParticleSystem.ForceOverLifetimeModule windForce;

    [SerializeField] private float windStrength = 500f; 
    [SerializeField] private float changeInterval = 1f; 

    private float nextChangeTime;
    private Vector2 currentWind;

    void Start()
    {
        rainSystem = GetComponent<ParticleSystem>();

        windForce = rainSystem.forceOverLifetime;

        UpdateWind();
        nextChangeTime = Time.time + changeInterval;
    }

    void Update()
    {
        if (Time.time >= nextChangeTime)
        {
            UpdateWind();
            nextChangeTime = Time.time + changeInterval; 
        }
        windForce.x = currentWind.x;
    }

    private void UpdateWind()
    {
        float randomX = Random.Range(-1f, 1f);
        float randomY = Random.Range(-1f, 1f);
        currentWind = new Vector2(randomX, randomY).normalized * windStrength;
    }
}

using UnityEngine;
using System.Collections;

public class LightningController : MonoBehaviour
{
    [SerializeField] private UnityEngine.Rendering.Universal.Light2D _lightningLight;
    [SerializeField] private AudioSource _thunderSound;
    [SerializeField] private float _minWaitBetweenLightning = 5f;
    [SerializeField] private float _maxWaitBetweenLightning = 12f;
    [SerializeField] private float _flashDuration = 0.1f;
 
    void Start()
    {
        StartCoroutine(StartLightningAfterDelay(_minWaitBetweenLightning));
    }

    IEnumerator StartLightningAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        while (true)
        {
            TriggerLightning();
            
            float waitTime = Random.Range(_minWaitBetweenLightning, _maxWaitBetweenLightning);
            
            yield return new WaitForSeconds(waitTime);
        }
    }

    void TriggerLightning()
    {
        StartCoroutine(LightningFlash());
    }

    IEnumerator LightningFlash()
    {
        _lightningLight.enabled = true;
        _lightningLight.intensity = 5f;

        _thunderSound.Play();

        yield return new WaitForSeconds(_flashDuration);

        for (float t = 0; t < 1; t += Time.deltaTime / 0.5f)
        {
            _lightningLight.intensity = Mathf.Lerp(5f, 0f, t);
            yield return null;
        }

        _lightningLight.intensity = 0f; 
        _lightningLight.enabled = false;
    }
}

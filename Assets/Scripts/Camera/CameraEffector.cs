using System;
using Cinemachine;
using UnityEngine;

public class CameraEffector : Singleton<CameraEffector>
{
    private CinemachineVirtualCamera _cinemachineVirtualCamera;

    private float _shakeIntensity = 2f;
    private float _shakeTime = 0.2f;

    private float _shakeTimer;
  
    protected override void Awake()
    {
        _cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        
        base.Awake();
    }

    private void LateUpdate()
    {
        if (_shakeTimer > 0)
        {
            _shakeTimer -= Time.deltaTime;

            if (_shakeTimer <= 0)
            {
                StopShake();
            }
        }
    }

    public void ShakeCamera()
    {
        var _cbmcp = _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _cbmcp.m_AmplitudeGain = _shakeIntensity;

        _shakeTimer = _shakeTime;
    }

    private void StopShake()
    {
        var _cbmcp = _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _cbmcp.m_AmplitudeGain = 0;

        _shakeTimer = 0;
    }
    
    
}
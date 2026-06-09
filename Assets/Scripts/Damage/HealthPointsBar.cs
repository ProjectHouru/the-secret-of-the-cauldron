using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class HealthPointsBar : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _fillHealthPointsBar;
    [SerializeField] private HealthPoint _healthPoint;

    private void OnEnable()
    {
        _healthPoint.OnDecreaseValue += ChangeHealthPoints;
        _healthPoint.OnIncreaseValue += ChangeHealthPoints;
    }
    
    private void OnDisable()
    {
        _healthPoint.OnDecreaseValue -= ChangeHealthPoints;
        _healthPoint.OnIncreaseValue -= ChangeHealthPoints;
    }

    private void ChangeHealthPoints()
    {
        _fillHealthPointsBar.size = new Vector2((float)_healthPoint.CurrentValue / _healthPoint.MaxValue, 1);
    }
}

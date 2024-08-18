using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeController : MonoBehaviour
{
    public float MaxLife => _maxLife;
    [SerializeField] private float _maxLife;
    public float CurrentLife => _currentLife;
    private float _currentLife;

    private void Start()
    {
        _currentLife = _maxLife;
    }

    public void UpdateLife(float lifeUpdate)
    {
        _currentLife += lifeUpdate;
        if (_currentLife <= 0) Die();
        else if (_currentLife > _maxLife) _currentLife = _maxLife;
        Debug.Log($"Life updated: {_currentLife}");
    }

    private void Die()
    {
        Debug.Log($"{name} died");
        Destroy(gameObject);
    }
}

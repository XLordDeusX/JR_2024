using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LifeController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI dmgPercentage;
    public float MaxLife => _maxLife;
    [SerializeField] private float _maxLife;
    public float CurrentLife => _currentLife;
    private float _currentLife;

    public float Resistance => _resistance;
    [SerializeField][Range(0,1)] float _resistance;
    public float DamagePercentage => _damagePercentage;
    private float _damagePercentage;

    private void Start()
    {
        _currentLife = _maxLife;
    }

    private void Update()
    {
        dmgPercentage.text = _damagePercentage.ToString();
    }

    public void UpdateLife(float lifeUpdate)
    {
        //_currentLife += lifeUpdate;
        //if (_currentLife <= 0) Die();
        //else if (_currentLife > _maxLife) _currentLife = _maxLife;
        //Debug.Log($"Life updated: {_currentLife}");

        if (DamagePercentage >= 1) Die();
        _damagePercentage += -lifeUpdate * (1 - _resistance)/10;
    }

    private void Die()
    {
        Debug.Log($"{name} died");
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LifeController : MonoBehaviour
{
    [SerializeField] Rigidbody2D _rb;
        public float Resistance => _resistance;
    [SerializeField][Range(0,1)] float _resistance;
    public float DamagePercentage => _damagePercentage;
    private float _damagePercentage;

    public void GetDamage(float damageUpdate, Vector2 push)
    {
        if (DamagePercentage < 1) push *= DamagePercentage;
        _rb.AddForce(push);
        _damagePercentage += -damageUpdate * (1 - _resistance)/10;
    }

    public void UpdateLife(float damageUpdate) => _damagePercentage += -damageUpdate * (1 - _resistance) / 10;
}

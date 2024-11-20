using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesPlatform : MonoBehaviour
{
    [SerializeField] private float damage;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Character"))
            collision.gameObject.GetComponent<DamageController>().GetDamage(damage);
    }
}

using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BothWaysPlatform : MonoBehaviour
{
    private PlatformEffector2D _effector2D;
    private float startTime;
    [SerializeField] private float effectTime;

    void Start()
    {
        _effector2D = GetComponent<PlatformEffector2D>();
    }

    void Update()
    {
        if (Input.GetAxisRaw("Vertical") > 0)
        {
            startTime = Time.time;
            _effector2D.rotationalOffset = 180;
        }
        if (Time.time >= startTime + effectTime)
            _effector2D.rotationalOffset = 0;
    }
}

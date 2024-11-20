using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DamageController : MonoBehaviour
{
    [SerializeField] PhotonView _pv;
    [SerializeField] Rigidbody2D _rb;
        public float Resistance => _resistance;
    [SerializeField][Range(0,1)] float _resistance;
    public float DamagePercentage => _damagePercentage;
    private float _damagePercentage;

    public void GetDamage(float damageUpdate, Vector2 push)
    {
        _pv.RPC("GetPushed", RpcTarget.AllBuffered, push);
        _damagePercentage += damageUpdate * (1 - _resistance)/10;
    }

    public void GetDamage(float damageUpdate) => _damagePercentage += damageUpdate * (1 - _resistance) / 10;

    [PunRPC]
    private void GetPushed(Vector2 push)
    {
        if (_damagePercentage < 1) push *= _damagePercentage;
        _rb.AddForce(push);
    }
}

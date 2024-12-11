using Photon.Pun;
using UnityEngine;

public class Limits : MonoBehaviour
{
    MatchManager _matchManager;

    private void Start()
    {
        _matchManager = GameManager.Instance.MatchManager;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager.Instance.PV.RPC("UpdateScore", RpcTarget.AllBuffered, collision.gameObject.GetComponent<Character>().Team != 1);
        collision.gameObject.GetComponent<DamageController>().PV.RPC("RestartDamage", RpcTarget.AllBuffered);
        _matchManager.RespawnPlayer(collision.gameObject);
    }
}

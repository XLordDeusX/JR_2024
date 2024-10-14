using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Limits : MonoBehaviour
{
    [SerializeField] MatchManager _matchManager;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        _matchManager.Updatescore(collision.gameObject.GetComponent<Character>().Team != 1);
        _matchManager.RespawnPlayer(collision.gameObject);
    }
}

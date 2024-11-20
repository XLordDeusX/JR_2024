using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Limits : MonoBehaviour
{
    [SerializeField] MatchManager _matchManager;

    private void Start()
    {
        _matchManager = FindObjectOfType<MatchManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _matchManager.Updatescore(collision.gameObject.GetComponent<Character>().Team != 1);
        _matchManager.RespawnPlayer(collision.gameObject);
    }
}

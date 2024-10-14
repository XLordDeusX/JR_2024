using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testScript : MonoBehaviour
{

    [SerializeField] MatchManager _matchManager;
    [SerializeField] GameObject charPrefab;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) CreatePlayer();
    }

    private void CreatePlayer()
    {
        _matchManager.GetNewPlayer(Instantiate(charPrefab, Vector3.zero, Quaternion.identity).GetComponent<Character>());
        Debug.Log("New character created");
    }
}

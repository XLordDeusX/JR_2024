using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] MatchManager matchManager;


    private void Start()
    {
        matchManager.GetNewPlayer(PhotonNetwork.Instantiate(playerPrefab.name, new Vector2(-5, 4), Quaternion.identity).GetComponent<Character>());
    }
}

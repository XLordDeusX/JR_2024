using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    PhotonView pv;
    int playerIndex;
    GameObject player;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }
    private void Start()
    {
        player = PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);

        playerIndex = PhotonNetwork.CurrentRoom.PlayerCount;

        pv.RPC("AssignTeam", RpcTarget.AllBuffered);

        GameManager.Instance.AddPlayer(player.GetComponent<Character>());
    }

    [PunRPC]
    private void AssignTeam()
    {
        PhotonView playerPV = player.GetComponent<PhotonView>();

        if(playerPV != null) 
        {
            playerPV.gameObject.GetComponent<Character>().SetTeam(playerIndex%2 +1); 
        }
    }
}

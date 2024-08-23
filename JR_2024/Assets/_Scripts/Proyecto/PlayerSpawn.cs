using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    SpriteRenderer playerSR;
    GameObject player;
    PhotonView pv;

    [SerializeField] Transform[] _spawnpoints;

    private void Start()
    {
        player = PhotonNetwork.Instantiate(playerPrefab.name, new Vector2(-5, 4), Quaternion.identity);
        playerSR = player.GetComponent<SpriteRenderer>();
        pv = GetComponent<PhotonView>();

        pv.RPC("SetColor", RpcTarget.AllBuffered);
        player.transform.SetPositionAndRotation(_spawnpoints[PhotonNetwork.CurrentRoom.PlayerCount-1].position, Quaternion.identity);
    }

    [PunRPC]
    private void SetColor()
    {
        switch (PhotonNetwork.CurrentRoom.PlayerCount)
        {
            case 1:
                playerSR.color = Color.white;
                break;
            case 2:
                playerSR.color = Color.blue;
                break;
        }
    }
}

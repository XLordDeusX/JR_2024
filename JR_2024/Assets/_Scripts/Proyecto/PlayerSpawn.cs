using Photon.Pun;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    PhotonView pv;
    GameObject player;


    private void Start()
    {
        pv = GetComponent<PhotonView>();
    }

#if UNITY_EDITOR
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) CreatePlayer();
    }
#endif

    public void CreatePlayer()
    {
        player = PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);

        int playerIndex = PhotonNetwork.CurrentRoom.PlayerCount;

        pv.RPC("AssignTeam", RpcTarget.AllBuffered, player.GetComponent<PhotonView>().ViewID, playerIndex);
    }

    [PunRPC]
    private void AssignTeam(int playerViewID, int playerIndex)
    {
        PhotonView playerPV = PhotonView.Find(playerViewID);
        if(playerPV != null) 
        {
            FindObjectOfType<MatchManager>().GetNewPlayer(playerPV.gameObject.GetComponent<Character>());
        }
    }
}

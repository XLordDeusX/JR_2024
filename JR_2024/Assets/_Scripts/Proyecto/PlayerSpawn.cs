using Photon.Pun;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    PhotonView pv;


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
        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);


        pv.RPC("SetNickname", RpcTarget.AllBuffered, player.GetComponent<PhotonView>().ViewID);
        pv.RPC("AssignTeam", RpcTarget.AllBuffered, player.GetComponent<PhotonView>().ViewID);
    }

    [PunRPC]
    private void AssignTeam(int photonViewID)
    {
        PhotonView playerPV = PhotonView.Find(photonViewID);
        if(playerPV != null)
            FindObjectOfType<MatchManager>().GetNewPlayer(playerPV.gameObject.GetComponent<Character>());
    }

    [PunRPC]
    private void SetNickname(int photonViewID)
    {
        PhotonView playerPV = PhotonView.Find(photonViewID);
        if (playerPV != null)
        {
            Character character = playerPV.gameObject.GetComponent<Character>();

            //string nickname = CheckNickname(playerPV);

            if (playerPV.IsMine) character.SetNickname(PhotonNetwork.NickName);
            else character.SetNickname(playerPV.Owner.NickName);
        }
    }

    private void CheckNickname(PhotonView ownPV)
    {
        //for (int n = 0; n < PhotonNetwork.PlayerList.Length; n++)
        //    if (PhotonNetwork.GetPhotonView(PhotonNetwork.PlayerList[n].)  != ownPV && PhotonNetwork.PlayerList[n].NickName == PhotonNetwork.NickName)
        //    {
        //        PhotonNetwork.NickName = $"{PhotonNetwork.NickName}{Random.Range(1, GameManager.Instance.MatchManager.PlayersPerTeam*2)}";
        //        CheckNickname();
        //    }
        //return PhotonNetwork.NickName;
    }
}

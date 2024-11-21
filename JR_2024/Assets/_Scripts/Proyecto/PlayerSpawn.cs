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

            //string nickname = CheckNickname(photonViewID);

            if (playerPV.IsMine) character.SetNickname(PhotonNetwork.NickName);
            else character.SetNickname(playerPV.Owner.NickName);
        }
    }

    //private string CheckNickname(int photonViewID)
    //{
    //    for (int n = 0; n < PhotonNetwork.PlayerList.Length-1; n++)
    //        if (!PhotonNetwork.GetPhotonView(photonViewID).IsMine && PhotonNetwork.PlayerList[n].NickName == PhotonNetwork.NickName)
    //        {
    //            PhotonNetwork.NickName = $"{PhotonNetwork.NickName}{Random.Range(1, GameManager.Instance.MatchManager.PlayersPerTeam * 2)}";
    //            return CheckNickname();
    //        }
    //    return PhotonNetwork.NickName;
    //}
}

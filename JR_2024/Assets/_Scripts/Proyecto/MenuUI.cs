using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviourPunCallbacks
{
    [SerializeField] Button createButton;
    [SerializeField] Button joinButton;

    [SerializeField] TMPro.TMP_InputField createInput;
    [SerializeField] TMPro.TMP_InputField joinInput;
    [SerializeField] TMPro.TMP_InputField nicknameInput;

    private void Awake()
    {
        createButton.onClick.AddListener(CreateRoom);
        joinButton.onClick.AddListener(JoinRoom);
    }

    private void OnDestroy()
    {
        createButton.onClick.RemoveAllListeners();
        joinButton.onClick.RemoveAllListeners();
    }

    public void CreateRoom()
    {
        if (nicknameInput.text != string.Empty) PhotonNetwork.NickName = nicknameInput.text;
        else
        {
            Debug.Log("Please choose a nickname");
            return;
        }
        RoomOptions roomConfigurations = new RoomOptions();
        roomConfigurations.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(createInput.text, roomConfigurations);
    }

    public void JoinRoom()
    {
        if (nicknameInput.text != string.Empty) PhotonNetwork.NickName = nicknameInput.text;
        else
        {
            Debug.Log("Please choose a nickname");
            return;
        }
        PhotonNetwork.JoinRoom(joinInput.text);
    }

    public override void OnJoinedRoom()
    {
        
        PhotonNetwork.LoadLevel("TestRoom");
    }

    public override void OnConnectedToMaster()
    {

    }

    public override void OnLeftRoom()
    {
        if(PhotonNetwork.IsMasterClient)
            PhotonNetwork.SetMasterClient(PhotonNetwork.MasterClient.GetNext());
        else
        {
            Time.timeScale = 0;
        }
    }
}

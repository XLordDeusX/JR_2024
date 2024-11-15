using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviourPunCallbacks
{
    [SerializeField] Button createButton;
    [SerializeField] Button joinButton;

    [SerializeField] TMPro.TMP_InputField createInput;
    [SerializeField] TMPro.TMP_InputField joinInput;

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
        RoomOptions roomConfigurations = new RoomOptions();
        roomConfigurations.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(createInput.text, roomConfigurations);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinInput.text);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("TestRoom");
    }
}

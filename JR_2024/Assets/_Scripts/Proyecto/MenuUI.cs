using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using Photon.Realtime;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviourPunCallbacks
{
    [SerializeField] Button createButton;
    [SerializeField] Button joinButton;

    [SerializeField] TMP_InputField createInput;
    [SerializeField] TMP_InputField joinInput;
    [SerializeField] TMP_InputField nicknameInput;

    [SerializeField] TextMeshProUGUI nicknameMessage;
    [SerializeField] TextMeshProUGUI roomCodeMessage;
    [SerializeField] float messageWaitTime;
    [SerializeField] float messageFadingTime;

    private void Awake()
    {
        createButton.onClick.AddListener(CreateRoom);
        joinButton.onClick.AddListener(JoinRoom);
        nicknameMessage.CrossFadeColor(Color.clear, 0f, true, true);
        roomCodeMessage.CrossFadeColor(Color.clear, 0f, true, true);
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
            StopCoroutine(ShowNicknameMessage());
            StartCoroutine(ShowNicknameMessage());
            return;
        }
        string roomCode;
        if (createInput.text != string.Empty) roomCode = createInput.text;
        else
        {
            StopCoroutine(ShowRoomCodeMessage());
            StartCoroutine(ShowRoomCodeMessage());
            return;
        }
        RoomOptions roomConfigurations = new RoomOptions();
        roomConfigurations.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(roomCode, roomConfigurations);
    }

    public void JoinRoom()
    {
        if (nicknameInput.text != string.Empty) PhotonNetwork.NickName = nicknameInput.text;
        else
        {
            StopCoroutine(ShowNicknameMessage());
            StartCoroutine(ShowNicknameMessage());
            return;
        }
        PhotonNetwork.JoinRoom(joinInput.text);
    }

    public override void OnJoinedRoom()
    {
        
        PhotonNetwork.LoadLevel("TestRoom");
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

    IEnumerator ShowNicknameMessage()
    {
        nicknameMessage.CrossFadeColor(Color.red, .25f, true, true);
        yield return new WaitForSeconds(messageWaitTime);
        nicknameMessage.CrossFadeColor(Color.clear, messageFadingTime, true, true);
    }

    IEnumerator ShowRoomCodeMessage()
    {
        roomCodeMessage.CrossFadeColor(Color.red, .25f, true, true);
        yield return new WaitForSeconds(messageWaitTime);
        roomCodeMessage.CrossFadeColor(Color.clear, messageFadingTime, true, true);
    }
}

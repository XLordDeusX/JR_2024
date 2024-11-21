using Photon.Pun;
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

    [SerializeField] TextMeshProUGUI messageText;
    [SerializeField] float messageWaitTime;
    [SerializeField] float messageFadingTime;

    private void Awake()
    {
        createButton.onClick.AddListener(CreateRoom);
        joinButton.onClick.AddListener(JoinRoom);
        messageText.CrossFadeColor(Color.clear, 0f, true, true);
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
            StopCoroutine(ShowMessage());
            StartCoroutine(ShowMessage());
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
            StopCoroutine(ShowMessage());
            StartCoroutine(ShowMessage());
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

    IEnumerator ShowMessage()
    {
        messageText.CrossFadeColor(Color.red, .25f, true, true);
        yield return new WaitForSeconds(messageWaitTime);
        messageText.CrossFadeColor(Color.clear, messageFadingTime, true, true);
    }
}

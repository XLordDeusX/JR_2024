using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField] GameObject playersInfo;
    [SerializeField] GameObject infoPrefab;
    [SerializeField] TextMeshProUGUI timer;
    private float startTime;
    private float matchDuration;

    public void SetStartTime() => startTime = (float)PhotonNetwork.Time;
    private void Awake()
    {
        matchDuration = GameManager.Instance.MatchManager.MatchTime;
    }

    private void Update()
    {
        if(GameManager.Instance.MatchManager.IsStarted)
            UpdateTimer();
    }

    public void AddPlayer(Character newPlayer)
    {
        Instantiate(infoPrefab, playersInfo.transform).GetComponent<UIInfo>().SetValues(newPlayer);
    }

    [PunRPC]
    private void UpdateTimer()
    {
        Debug.Log(((float)PhotonNetwork.Time - startTime));
        float elapsedTime = matchDuration - ((float)PhotonNetwork.Time - startTime);

        float minutes = elapsedTime / 60;
        float seconds = elapsedTime % 60;
        timer.text = string.Format("{0:00}:{1:00}", (int)minutes, (int)seconds);
    }
}

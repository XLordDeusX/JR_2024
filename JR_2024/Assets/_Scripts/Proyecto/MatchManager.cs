using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Photon.Pun;
using Unity.VisualScripting;
using System.Linq;

public class MatchManager : MonoBehaviour
{
    private List<Character> players = new List<Character>();
    private List<Character> team1Members = new List<Character>();
    private List<Character> team2Members = new List<Character>();
    private int team1Score;
    private int team2Score;
    [SerializeField] int targetScore;
    [SerializeField] int playersPerTeam;

    private float startTime;
    [SerializeField] float matchTime;
    public bool IsStarted => isStarted;
    private bool isStarted;

    [SerializeField] CameraScript cam;
    [SerializeField] HUD hud;

    private void Awake()
    {
        GameManager.Instance.MatchManager = this;
    }

    private void Update()
    {
        if (isStarted)
        {
            if(Time.time >= startTime + matchTime) EndMatch(false);
        }
        else if (playersPerTeam * 2 == players.Count) SetMatch();
    }

    public void SetCam(CameraScript newCam) => cam = newCam;

    public void SetMatch()
    {
        StartCoroutine(GameManager.Instance.SetGameplayUI());
        cam.Start();
        team1Score = 0;
        team2Score = 0;
        startTime = Time.time;
        isStarted = true;
        SpawnPlayers();
    }

    public void GetNewPlayer(Character newPlayer)
    {
        players.Add(newPlayer);
        hud.AddPlayer(newPlayer);
        cam.Players.Add(newPlayer.gameObject);
        newPlayer.SetState(CharacterState.Ready);
        AsignTeam(newPlayer);
    }

    private void SpawnPlayers()
    {
        for (int n = 0; n < players.Count; n++)
        {
            cam.Players.Add(players[n].gameObject);
            RespawnPlayer(players[n].gameObject, GameObject.FindGameObjectsWithTag("Spawnpoint")[n].transform);
        }
    }

    public void RespawnPlayer(GameObject playerToRespawn)
    {
        Character player = playerToRespawn.GetComponent<Character>();
        GameObject[] tGO = GameObject.FindGameObjectsWithTag("Spawnpoint");
        Transform t = tGO[Random.Range(0, tGO.Length)].transform;

        playerToRespawn.transform.SetPositionAndRotation(t.position, t.rotation);
        StartCoroutine(player.Respawn());
    }

    public void RespawnPlayer(GameObject playerToRespawn, Transform t)
    {
        Character player = playerToRespawn.GetComponent<Character>();

        playerToRespawn.transform.SetPositionAndRotation(t.position, t.rotation);
        StartCoroutine(player.Respawn());
    }

    public void Updatescore(bool team1Scored)
    {
        if (team1Scored) team1Score++;
        else team2Score++;

        if (targetScore <= team1Score || targetScore <= team2Score) EndMatch(team1Scored);
    }

    private void EndMatch(bool byScore)
    {
        if (byScore)
        {
            bool team1Won;
            if (team1Score >= targetScore) team1Won = true;
            else team1Won = false;
            GameManager.Instance.EndMatch(team1Won);
        }
        else GameManager.Instance.EndMatch();
        isStarted = false;
    }
    
    private void AsignTeam(Character newPlayer)
    {
        int dif = team2Members.Count - team1Members.Count;
        if (dif >= 0)
        {
            team1Members.Add(newPlayer);
            newPlayer.SetTeam(1);
        }
        else
        {
            team2Members.Add(newPlayer);
            newPlayer.SetTeam(2);
        }
    }
}

using System.Collections.Generic;
using System.Collections;
using UnityEngine;

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

    [SerializeField] Transform[] _spawnpoints;
    [SerializeField] CameraScript cam;

    private void Update()
    {
        if (isStarted)
        {
            if(Time.time >= startTime + matchTime) EndMatch(false);
        }
        else if (playersPerTeam * 2 == team1Members.Count + team2Members.Count) SetMatch();
    }

    public void SetMatch()
    {
        StartCoroutine(GameManager.Instance.SetGameplayUI());
        cam.Start();
        SpawnPlayers();
        team1Score = 0;
        team2Score = 0;
        startTime = Time.time;
        isStarted = true;
    }

    public void GetNewPlayer(Character newPlayer)
    {
        players.Add(newPlayer);
        newPlayer.SetState(CharacterState.Ready);
        AsignTeam(newPlayer);
    }

    private void SpawnPlayers()
    {
        foreach(Character player in players)
        {
            cam.Players.Add(player.gameObject);
            RespawnPlayer(player.gameObject);
        }
    }

    public void RespawnPlayer(GameObject playerToRespawn)
    {
        Character player = playerToRespawn.GetComponent<Character>();
        int i = players.IndexOf(player);
        playerToRespawn.transform.SetPositionAndRotation(_spawnpoints[i].position, Quaternion.identity);
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

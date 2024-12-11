using Photon.Pun;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public SceneManagerScript SceneManager => _sceneManager;
    private SceneManagerScript _sceneManager;


    [SerializeField] GameObject afterMatchUI;
    [SerializeField] GameObject gameplayUI;
    [SerializeField] TextMeshProUGUI resultText;
    [SerializeField] GameObject fighTextGO;
    [SerializeField] Button rematchButton;
    [SerializeField] Button mainMenuButton;

    public MatchManager MatchManager;
    public PhotonView PV => _pv;
    private PhotonView _pv;

    public int Team1Score => team1Score;
    private int team1Score;
    public int Team2Score => team2Score;
    private int team2Score;
    public int TargetScore => targetScore;
    [SerializeField] int targetScore;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        DontDestroyOnLoad(gameObject);

        GetComponents();

        rematchButton.onClick.AddListener(OnRematchButtonPressed);
        mainMenuButton.onClick.AddListener(OnMainMenuButtonPressed);
    }

    [PunRPC]
    public void UpdateScore(bool team1Scored)
    {
        if (team1Scored) team1Score++;
        else team2Score++;

        if (team1Score >= targetScore || team2Score >= targetScore)
        {
            bool team1Won = false;
            if (Team1Score >= TargetScore) team1Won = true;
            EndMatch(team1Won);
        }
    }

    public void EndMatch()
    {
        resultText.text = "DRAW";
        resultText.color = Color.white;
        ChangeUI();
        MatchManager.Restart();
    }

    private void EndMatch(bool team1Won)
    {
        SetResult(team1Won);
        ChangeUI();
        MatchManager.Restart();
    }

    [PunRPC]
    public void RestartScores()
    {
        team1Score = 0;
        team2Score = 0;
    }

    public void OnMainMenuButtonPressed()
    {
        _pv.RPC("LoadMenu", RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void LoadMenu()
    {
        Debug.Log("Main menu loaded");
        _sceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        Debug.Log("Exit game");
        Application.Quit();
    }

    public void GetComponents()
    {
        _sceneManager = GetComponent<SceneManagerScript>();
        _pv = GetComponent<PhotonView>();
    }

    public IEnumerator SetGameplayUI()
    {
        gameplayUI.SetActive(true);
        yield return new WaitForSeconds(1f);
        fighTextGO.SetActive(false);
    }

    private void ChangeUI()
    {
        gameplayUI.SetActive(!gameplayUI.activeInHierarchy);
        afterMatchUI.SetActive(!afterMatchUI.activeInHierarchy);
    }

    private void SetResult(bool team1Won)
    {
        if (team1Won)
        {
            resultText.text = "TEAM 1 WON";
            resultText.color = Color.red;
        }
        else
        {

            resultText.text = "TEAM 2 WON";
            resultText.color = Color.blue;
        } 
    }

    public void OnRematchButtonPressed()
    {
        _pv.RPC("Rematch", RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void Rematch()
    {
        ChangeUI();
        FindObjectOfType<MatchManager>().SetMatch();
    }
}

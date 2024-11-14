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

    private MatchManager matchManager;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        GetComponents();

        rematchButton.onClick.AddListener(Rematch);
        mainMenuButton.onClick.AddListener(LoadMainMenu);
    }

    public void LoadMainMenu()
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
    }

    public void AddPlayer(Character newPlayer)
    {
        if (PhotonNetwork.IsMasterClient) FindObjectOfType<MatchManager>().GetNewPlayer(newPlayer);
    }
    
    public void StartMatch()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            matchManager = PhotonNetwork.Instantiate("MatchManager", Vector3.zero, Quaternion.identity).GetPhotonView().gameObject.GetComponent<MatchManager>();
            matchManager.SetCam(PhotonNetwork.Instantiate("Camera", new Vector3(0, 0, -10), Quaternion.identity).GetPhotonView().gameObject.GetComponent<CameraScript>());
            Debug.Log("aaa");
        }
    }

    public void EndMatch()
    {
        resultText.text = "DRAW";
        resultText.color = Color.white;
        ChangeUI();
    }

    public void EndMatch(bool team1Won)
    {
        SetResult(team1Won);
        ChangeUI();
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

    public void Rematch()
    {
        ChangeUI();
        FindObjectOfType<MatchManager>().SetMatch();
    }
}

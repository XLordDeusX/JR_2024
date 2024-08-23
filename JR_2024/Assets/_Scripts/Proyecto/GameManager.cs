using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public SceneManagerScript SceneManager => _sceneManager;
    private SceneManagerScript _sceneManager;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        GetComponents();
    }

    public void LoadGame()
    {
        Debug.Log("Game loaded");
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    public int CurrentScene => _currentScene;
    private int _currentScene;

    private void Awake()
    {
        _currentScene = SceneManager.GetActiveScene().buildIndex;
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(_currentScene++);
    }

    public void LoadScene(int newScene)
    {
        _currentScene = newScene;
        SceneManager.LoadScene(newScene);
    }
}

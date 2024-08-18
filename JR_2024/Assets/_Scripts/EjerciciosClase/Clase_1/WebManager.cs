using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WebManager : MonoBehaviour
{
    [Header("SERVER")]
    [SerializeField] TextMeshProUGUI _serverTimerText;
    [SerializeField] Image _serverStateFeedback;
    [SerializeField] int _serverTimeForAnswer;
    [SerializeField] float _serverCurrentTime;
    [SerializeField] bool _serverState;
    
    [Header("PLAYER")]
    [SerializeField] TextMeshProUGUI _playerTimerText;
    [SerializeField] Image _playerStateFeedback;
    [SerializeField] int _playerTimeForAnswer;
    [SerializeField] float _playerCurrentTime;
    [SerializeField] bool _playerState;


    void Start()
    {
        _playerState = true;
        _playerCurrentTime = _playerTimeForAnswer;

        _serverState = true;
        _serverCurrentTime = _serverTimeForAnswer;
    }

    void Update()
    {
        StateChecker();
    }
    
    public void StateChecker()
    {
        if (!_playerState)
        {
            _playerStateFeedback.color = Color.red;
            _playerCurrentTime -= Time.deltaTime;
            CountDown(_playerCurrentTime, _playerTimerText);
            if (_playerCurrentTime <= 0)
            {
                _playerCurrentTime = 0;
                _playerTimerText.text = _playerCurrentTime.ToString();
                Debug.Log("Jugador se desconecto");
            }
        }
        else
        {
            _playerCurrentTime = _playerTimeForAnswer;
            _playerStateFeedback.color = Color.green;
            _playerTimerText.text = "-";
        }

        if (!_serverState)
        {
            _serverStateFeedback.color = Color.red;
            _serverCurrentTime -= Time.deltaTime;
            CountDown(_serverCurrentTime, _serverTimerText);
            if (_serverCurrentTime <= 0)
            {
                _serverCurrentTime = 0;
                _serverTimerText.text = _serverCurrentTime.ToString();
                Debug.Log("Servidor se cayo");
                _playerStateFeedback.color = Color.red;
            }
        }
        else
        {
            _serverCurrentTime = _serverTimeForAnswer;
            _serverStateFeedback.color = Color.green;
            _serverTimerText.text = "-";
        }
    }
    public void PlayerStateChange()
    {
        _playerState = !_playerState;
    }    
    public void ServerStateChange()
    {
        _serverState = !_serverState;
    }
    void CountDown(float time, TextMeshProUGUI text)
    {
        // Mostrar solo los segundos sin decimales
        int seconds = Mathf.FloorToInt(time % 60);
        string timeText = seconds.ToString("00"); // Formato para mostrar dos dígitos

        // Asignar el texto al objeto de la UI que muestra el tiempo
        text.text = timeText; // textoTiempo es el objeto Text de tu interfaz de usuario
    }
}

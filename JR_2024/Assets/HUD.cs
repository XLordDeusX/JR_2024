using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField] GameObject playersInfo;
    [SerializeField] GameObject infoPrefab;


    public void AddPlayer(Character newPlayer)
    {
        Instantiate(infoPrefab, playersInfo.transform).GetComponent<UIInfo>().SetValues(newPlayer);
    }
}

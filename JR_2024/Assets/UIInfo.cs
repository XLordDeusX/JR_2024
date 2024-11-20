using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIInfo : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nickname;
    [SerializeField] TextMeshProUGUI damagePercentage;
    [SerializeField] Image damageFill;

    DamageController LC;

    public void SetValues(Character character)
    {
        LC = character.LifeController;
        nickname.text = character.Nickname;
    }

    private void Update()
    {
        damageFill.fillAmount = LC.DamagePercentage;
        damagePercentage.text = $"{(int)(LC.DamagePercentage*100)}%";
    }
}

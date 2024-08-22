using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "NewComboAttacks", menuName = "ScriptableObject/ComboInfo")]
public class ComboInfo : ScriptableObject
{
    public int attacksAmount;
    public float[] attacksTiming;
    public float[] attacksDamage;
    public float[] attacksRange;
    public float[] attacksForce;
    public float[] attacksRadius;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterView : MonoBehaviour
{
    private Animator _anim;
    private Character _character;

    public void Initialize()
    {
        _anim = GetComponent<Animator>();
        _character = GetComponent<Character>();
    }

    public void RunAnim(float value)
    {
        if (value < 0)
        {
            _anim.SetBool("isMoving", true);
            _character.transform.localScale = new Vector3(-1, 1, 0); 
        }
        else if(value > 0)
        {
            _anim.SetBool("isMoving", true);
            _character.transform.localScale = new Vector3(1, 1, 0);
        }
        else if(value == 0)
        {
            _anim.SetBool("isMoving", false);
        }
    }
    public void AttackAnim() { _anim.SetTrigger("Attack"); }
    public void AttackAnim(int value)
    {        
        _anim.SetInteger("ComboCount", value + 1);
    }

    public void JumpAnim() { _anim.SetTrigger("OnJump"); }

    public void LandingAnim() { _anim.SetTrigger("OnLand"); }

    public void RespawnAnim(bool state)
    {
        if(state == true) _anim.SetBool("isSpawning", state);
        else _anim.SetBool("isSpawning", state);
    }

}

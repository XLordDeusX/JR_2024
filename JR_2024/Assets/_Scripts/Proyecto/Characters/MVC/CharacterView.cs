using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterView : MonoBehaviour
{
    private Animator _anim;
    private Character _character;

    void Start()
    {
        _anim = GetComponent<Animator>();
        _character = GetComponent<Character>();
    }

    
    void Update()
    {
        RunAnim(_character.RB.velocity);
    }

    public void RunAnim(Vector2 dir)
    {
        if (dir.x < 0)
        {
            _anim.SetBool("isMoving", true);
            _character.transform.localScale = new Vector3(-1, 1, 0); 
        }
        else if(dir.x > 0)
        {
            _anim.SetBool("isMoving", true);
            _character.transform.localScale = new Vector3(1, 1, 0);
        }
        else if(dir.x == 0)
        {
            _anim.SetBool("isMoving", false);
        }
    }

    public void JumpAnim(bool action)
    {
        if (!action)
        {
            _anim.SetTrigger("OnJump");
        }
        else
        {
            _anim.SetTrigger("OnLand");
        }
    }

    public void RespawnAnim(bool state)
    {
        if(state == true) _anim.SetBool("isSpawning", state);
        else _anim.SetBool("isSpawning", !state);
    }

}

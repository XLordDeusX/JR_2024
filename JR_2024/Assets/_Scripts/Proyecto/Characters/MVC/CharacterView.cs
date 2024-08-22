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

    public void JumpAnim(Vector2 dir)
    {
        if (dir.y != 0)
        {
            _anim.SetBool("isMoving", true);
        }
        else
        {
            _anim.SetBool("isMoving", false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Character : MonoBehaviour
{
                //COMPONENTS
    public Rigidbody2D RB => _rb;
    private Rigidbody2D _rb;
    public Collider2D Collider => _collider;
    private Collider2D _collider;
    public LifeController LifeController => _lifeController;
    private LifeController _lifeController;

                //VARIABLES
    //Combat
    [SerializeField] LayerMask targetLayer;
    [HideInInspector] public float LastAttackTime;
    private int _lastComboAttack;
    //Movement
    private float _hor;
    private bool _isGrounded;
    private bool _hasDoubleJump;

                //STATS
    [Header("STATS")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float movementSpeed;
    [SerializeField] private Vector2 maxVelocity;
    [SerializeField] ComboInfo comboInfo;

    private void Start()
    {
        GetComponents();
    }

    private void Update()
    {
        GetInput();
        Move();
    }

    private void GetComponents()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _lifeController = GetComponent<LifeController>();
    }

    private void GetInput()
    {
        _hor = Input.GetAxis("Horizontal");
        if ((_hasDoubleJump || _isGrounded) && Input.GetKeyDown(KeyCode.Space)) Jump();
        if (Input.GetMouseButtonDown(0)) Attack();
    }

    #region MOVEMENT
    private void Move()
    {
        _rb.velocity = new Vector2(_hor * movementSpeed, _rb.velocity.y);
        if(_rb.velocity.x > maxVelocity.x) _rb.velocity = new Vector2(maxVelocity.x, _rb.velocity.y);
        if(_rb.velocity.y > maxVelocity.y) _rb.velocity = new Vector2(_rb.velocity.x, maxVelocity.y);
    }

    private void Jump()
    {
        _rb.velocity = new Vector2(_rb.velocity.x, 0);
        _rb.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);
        if (!_isGrounded) _hasDoubleJump = false;
        _isGrounded = false;
    }
    #endregion

    #region COMBAT

    private void Attack()
    {
        int attack = SelectAttack();
        _lastComboAttack = attack;
        Debug.Log($"Attack: {attack}");
        DealDamage(comboInfo.attacksDamage[attack], comboInfo.attacksRange[attack], comboInfo.attacksRadius[attack], comboInfo.attacksForce[attack]);
        LastAttackTime = Time.time;
    }
    private int SelectAttack()
    {
        if (Time.time <= LastAttackTime + comboInfo.attacksTiming[_lastComboAttack])
        {
            _lastComboAttack++;
            if (_lastComboAttack == comboInfo.attacksAmount) _lastComboAttack = 0;
            return _lastComboAttack;
        }
        else return 0;
    }

    private void DealDamage(float damage, float range, float radius, float force)
    {
        Collider2D[] victims = Physics2D.OverlapCircleAll((Vector2)transform.position + Vector2.right * transform.localScale.x * range, radius, targetLayer);
        foreach (Collider2D victim in victims)
        {
            if (victim.gameObject != gameObject)
            {
                Debug.Log(force * (victim.transform.position - transform.position));
                victim.GetComponent<Character>().GetDamage(damage, force * (victim.transform.position - transform.position));
            }
        }
    }
#endregion

    public void GetDamage(float damageTaken)
    {
        _lifeController.UpdateLife(-damageTaken);
        Debug.Log($"Took damage: {damageTaken}");
    }

    public void GetDamage(float damageTaken, Vector2 push)
    {
        _rb.AddForce(push * _lifeController.DamagePercentage);
        _lifeController.UpdateLife(-damageTaken);
        Debug.Log($"Took damage: {damageTaken}");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch(collision.gameObject.tag)
        {
            case "Ground":
                _isGrounded = true;
                _hasDoubleJump = true;
                break;
            case "Spikes":
                _isGrounded = true;
                _hasDoubleJump = true;
                break;
            //case "Limits":
            //    GetDamage(_lifeController.CurrentLife);
            //    break;
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere((Vector2)transform.position + Vector2.right * transform.localScale.x * comboInfo.attacksRange[0], comboInfo.attacksRadius[0]);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere((Vector2)transform.position + Vector2.right * transform.localScale.x * comboInfo.attacksRange[1], comboInfo.attacksRadius[1]);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)transform.position + Vector2.right * transform.localScale.x * comboInfo.attacksRange[2], comboInfo.attacksRadius[2]);
    }
}
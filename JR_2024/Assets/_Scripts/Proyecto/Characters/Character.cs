using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;


public enum CharacterState
{
    Spawning,
    Ready,
    OutOfFight,
}

public class Character : MonoBehaviour
{
                //COMPONENTS
    public Rigidbody2D RB => _rb;
    private Rigidbody2D _rb;
    public Collider2D Collider => _collider;
    private Collider2D _collider;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    public LifeController LifeController => _lifeController;
    private LifeController _lifeController;
    public PhotonView PV => _pv;
    private PhotonView _pv;

    

                //VARIABLES
    //Combat
    [SerializeField] LayerMask _targetLayer;
    [HideInInspector] public float LastAttackTime;
    private int _lastComboAttack;
    public Transform RefPoint => _refPoint;
    [SerializeField] Transform _refPoint;
    public int Team => _team;
    private int _team;

    private Color teamColor;

    private CharacterState currentState;

    public void SetTeam(int newTeam)
    {
        _team = newTeam;
        if (_team == 1) teamColor = Color.red;
        else teamColor = Color.blue;
        _spriteRenderer.color = teamColor;
    }

    public void SetState(CharacterState newState) => currentState = newState;

    //Movement
    private float _hor;
    private bool _isGrounded;
    private bool _hasDoubleJump;

                //STATS
    [Header("STATS")]
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _movementSpeed;
    [SerializeField] private Vector2 _maxVelocity;
    [SerializeField] ComboInfo _comboInfo;

    private void Awake()
    {
        GetComponents();
    }

    private void Update()
    {
        if(currentState == CharacterState.Ready && _pv.IsMine)
        {
            GetInput();
            Move();
        }
    }

    private void GetComponents()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _lifeController = GetComponent<LifeController>();
        _pv = GetComponent<PhotonView>();
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
        _rb.velocity = new Vector2(_hor * _movementSpeed, _rb.velocity.y);
        if(_rb.velocity.x > _maxVelocity.x) _rb.velocity = new Vector2(_maxVelocity.x, _rb.velocity.y);
        if(_rb.velocity.y > _maxVelocity.y) _rb.velocity = new Vector2(_rb.velocity.x, _maxVelocity.y);
    }

    private void Jump()
    {
        _rb.velocity = new Vector2(_rb.velocity.x, 0);
        _rb.AddForce(_jumpForce * Vector2.up, ForceMode2D.Impulse);
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
        DealDamage(_comboInfo.attacksDamage[attack], _comboInfo.attacksRange[attack], _comboInfo.attacksRadius[attack], _comboInfo.attacksForce[attack]);
        LastAttackTime = Time.time;
    }
    private int SelectAttack()
    {
        if (Time.time <= LastAttackTime + _comboInfo.attacksTiming[_lastComboAttack])
        {
            _lastComboAttack++;
            if (_lastComboAttack == _comboInfo.attacksAmount) _lastComboAttack = 0;
            return _lastComboAttack;
        }
        else return 0;
    }

    private void DealDamage(float damage, float range, float radius, float force)
    {
        Collider2D[] victims = Physics2D.OverlapCircleAll((Vector2)_refPoint.position + Vector2.right * transform.localScale.x * range, radius, _targetLayer);
        foreach (Collider2D victim in victims)
        {
            if (victim.gameObject != gameObject)
            {
                Transform refPoint = victim.GetComponent<Character>().RefPoint;
                //Debug.Log(force * (refPoint.position - _refPoint.position));
                victim.GetComponent<Character>().GetDamage(damage, force * (refPoint.position - _refPoint.position));
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

    public IEnumerator Respawn()
    {
        _animator.SetBool("isSpawning", true);
        _rb.simulated = false;
        SetState(CharacterState.Spawning);
        yield return new WaitForSeconds(2f);
        _animator.SetBool("isSpawning", false);
        _rb.simulated = true;
        SetState(CharacterState.Ready);
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
        Gizmos.DrawWireSphere((Vector2)_refPoint.position + Vector2.right * transform.localScale.x * _comboInfo.attacksRange[0], _comboInfo.attacksRadius[0]);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere((Vector2)_refPoint.position + Vector2.right * transform.localScale.x * _comboInfo.attacksRange[1], _comboInfo.attacksRadius[1]);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)_refPoint.position + Vector2.right * transform.localScale.x * _comboInfo.attacksRange[2], _comboInfo.attacksRadius[2]);
    }
}
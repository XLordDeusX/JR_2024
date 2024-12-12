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
    private CharacterView _charView;
    public DamageController LifeController => _lifeController;
    private DamageController _lifeController;
    public PhotonView PV => _pv;
    private PhotonView _pv;
    [SerializeField] private CustomDict<AudioClips, AudioClip> _clips;
    public CustomDict<AudioClips, AudioClip> Clips => _clips;
    [SerializeField] private ParticleSystem _ps;

                //VARIABLES
    //Combat
    [SerializeField] LayerMask _targetLayer;
    [HideInInspector] public float LastAttackTime;
    [SerializeField] private int _lastComboAttack;
    public Transform RefPoint => _refPoint;
    [SerializeField] Transform _refPoint;
    public int Team => _team;
    private int _team;

    private Color teamColor;
    public string Nickname => _nickname;
    private string _nickname;

    public void SetNickname(string newNickname) => _nickname = newNickname;

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
    [SerializeField] private bool _isGrounded;
    [SerializeField] private bool _hasDoubleJump;

                //STATS
    [Header("STATS")]
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _movementSpeed;
    [SerializeField] private Vector2 _maxVelocity;
    [SerializeField] ComboInfo _comboInfo;

    private bool isAttacking;

    private void Awake()
    {
        GetComponents();
    }

    private void Update()
    {
        if(currentState == CharacterState.Ready && _pv.IsMine)
        {
            GetInput();
            _pv.RPC("Move", RpcTarget.AllBuffered);
        }
    }

    private void GetComponents()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _charView = GetComponent<CharacterView>();
        _charView.Initialize();
        _lifeController = GetComponent<DamageController>();
        _pv = GetComponent<PhotonView>();
        _clips.Initialize();
    }

    private void GetInput()
    {
        _hor = Input.GetAxis("Horizontal");
        if ((_hasDoubleJump || _isGrounded) && Input.GetKeyDown(KeyCode.Space)) 
        {
            _pv.RPC("Jump", RpcTarget.AllBuffered);
            _charView.JumpAnim();
            _ps.Play();
        }
        if (Input.GetMouseButtonDown(0))
        {
            _pv.RPC("Attack", RpcTarget.AllBuffered);
            _pv.RPC("AttackTest", RpcTarget.AllBuffered);             
        }
    }

    #region MOVEMENT
    [PunRPC]
    private void Move()
    {
        if (_hor != 0) _rb.velocity = new Vector2(_hor * _movementSpeed, _rb.velocity.y);
        if(_rb.velocity.x > _maxVelocity.x) _rb.velocity = new Vector2(_maxVelocity.x, _rb.velocity.y);
        if(_rb.velocity.y > _maxVelocity.y) _rb.velocity = new Vector2(_rb.velocity.x, _maxVelocity.y);
        _charView.RunAnim(_hor);
    }

    [PunRPC]
    private void Jump()
    {
        AudioManager.Instance.PlaySound(_clips.Find(AudioClips.Jump));
        _rb.velocity = new Vector2(_rb.velocity.x, 0);
        _rb.AddForce(_jumpForce * Vector2.up, ForceMode2D.Impulse);
        if (!_isGrounded) _hasDoubleJump = false;
        _isGrounded = false;
    }
    [PunRPC]
    private void Landing(bool playPS, bool playLanding)
    {
        _isGrounded = true;
        _hasDoubleJump = true;
        if(playLanding) _charView.LandingAnim();
        if (playPS && !_ps.isPlaying) _ps.Play();
    }
    #endregion

    #region COMBAT

    [PunRPC]
    private void AttackTest()
    {
        if(!isAttacking)
        {
            _charView.AttackAnim();
            StartCoroutine(AttackState());
        }
    }
    [PunRPC]
    private void Attack()
    {
        AudioManager.Instance.PlaySound(_clips.Find(AudioClips.Attack));
        int attack = SelectAttack();
        _lastComboAttack = attack;
        DealDamage(_comboInfo.attacksDamage[attack], _comboInfo.attacksRange[attack], _comboInfo.attacksRadius[attack], _comboInfo.attacksForce[attack]);
        LastAttackTime = Time.time;
        _charView.AttackAnim(_lastComboAttack);
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

    private IEnumerator AttackState()
    {
        isAttacking = true;
        yield return new WaitForSeconds(_comboInfo.attacksTiming[_lastComboAttack]);
        isAttacking = false;
    }

    private void DealDamage(float damage, float range, float radius, float force)
    {
        Collider2D[] victims = Physics2D.OverlapCircleAll((Vector2)_refPoint.position + Vector2.right * transform.localScale.x * range, radius, _targetLayer);
        foreach (Collider2D victim in victims)
        {
            if (victim.gameObject != gameObject)
            {
                Transform refPoint = victim.GetComponent<Character>().RefPoint;
                victim.GetComponent<DamageController>().GetDamage(damage, force * (refPoint.position - _refPoint.position));
            }
        }
    }
#endregion

    public IEnumerator Respawn()
    {
        //_charView.RespawnAnim(true);
        //_animator.SetBool("isSpawning", true);
        _rb.simulated = false;
        _rb.velocity = Vector2.zero;
        SetState(CharacterState.Spawning);
        yield return new WaitForSeconds(2f);
        //_charView.RespawnAnim(false);
        //_animator.SetBool("isSpawning", false);
        _rb.simulated = true;
        SetState(CharacterState.Ready);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch(collision.gameObject.tag)
        {
            case "Ground":
                _pv.RPC("Landing", RpcTarget.AllBuffered, true, true);
                break;
            case "Spikes":
                _pv.RPC("Landing", RpcTarget.AllBuffered, false, false);
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
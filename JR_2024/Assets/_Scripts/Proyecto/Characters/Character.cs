using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Character : MonoBehaviour
{
    public Rigidbody2D RB => _rb;
    private Rigidbody2D _rb;
    public Collider2D Collider => _collider;
    private Collider2D _collider;
    public LifeController LifeController => _lifeController;
    private LifeController _lifeController;

    private float hor;
    private bool grounded;
    private bool hasDoubleJump;


    [Header("STATS")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float movementSpeed;

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
        hor = Input.GetAxis("Horizontal");
        if ((hasDoubleJump || grounded) && Input.GetKeyDown(KeyCode.Space)) Jump();
    }

    private void Move()
    {
        _rb.velocity = new Vector2(hor * movementSpeed, _rb.velocity.y);
    }

    private void Jump()
    {
        _rb.velocity = new Vector2(_rb.velocity.x, 0);
        _rb.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);
        if (!grounded) hasDoubleJump = false;
        grounded = false;
    }

    public void GetDamage(float damageTaken)
    {
        _lifeController.UpdateLife(-damageTaken);
        Debug.Log($"Took damage: {damageTaken}");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch(collision.gameObject.tag)
        {
            case "Ground":
                grounded = true;
                hasDoubleJump = true;
                break;
            case "Spikes":
                grounded = true;
                hasDoubleJump = true;
                break;
            case "Limits":
                GetDamage(_lifeController.CurrentLife);
                break;
        }
    }
}

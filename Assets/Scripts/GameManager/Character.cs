using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public abstract class Character : MonoBehaviour
{
    public string characterName;
    public float speed = 5f;
    public float jumpPower = 10f;
    public float hp = 100f;
    public float gravity = 9.81f;
    public int PlayerID;

    public Rigidbody2D rb;
    public SpriteRenderer sr;
    protected bool isGrounded = true;
    public bool canMove = true;
    public bool isInvincible = false;

    [Header("Attacks")]
    public Attack dashAttack;


    protected void Init(string name)
    {
        characterName = name;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        dashAttack.Init(this);
    }

    protected virtual void Update()
    {
        ApplyGravity();
        OnDashAttack();
    }

    public virtual void Move(float direction, float speedScaler)
    {
       rb.linearVelocity = new Vector2(direction * speedScaler, rb.linearVelocity.y);
    }

    public virtual void Jump()
    {
        if (isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
            isGrounded = false;
        }
    }

    void OnDashAttack()
    {
        if (PlayerID == 1 && Input.GetKeyDown(KeyCode.Space))
        {
            dashAttack.Activate();
        }
        else if (PlayerID == 2 && Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            dashAttack.Activate();
        }
    }
    public virtual void TakeDamage(float damage)
    {
        hp -= damage;
        Debug.Log(characterName + " took " + damage + " damage! ");
    }

    protected virtual void ApplyGravity()
    {
        if (!isGrounded)
        {
            rb.linearVelocity += Vector2.down * gravity * Time.deltaTime;
        }
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0.5f)
        {
            isGrounded = true;
        }
    }

    protected void HandleInput()
    {
        float moveDir = 0f;

        if (PlayerID == 1)
        {
            if (Input.GetKey(KeyCode.A))
            {
                moveDir = -1f;
                sr.flipX = true;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                moveDir = 1f;
                sr.flipX = false;
            }
            if (Input.GetKeyDown(KeyCode.W)) Jump();
        }
        else if (PlayerID == 2)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                moveDir = -1f;
                sr.flipX = true;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                moveDir = 1f;
                sr.flipX = false;
            }
            if (Input.GetKeyDown(KeyCode.UpArrow)) Jump();
        }
        if (canMove)
        {
            Move(moveDir, speed);
        }
    }
}
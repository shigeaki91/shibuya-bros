using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Scripting.APIUpdating;

public abstract class Character : MonoBehaviour
{
    public string characterName;
    public float speed = 5f;
    public float moveDir = 0f;
    public float dashTimer = 0f;
    public float jumpPower = 10f;
    public float jumpTimer = 0f;
    public float jumpLag = 0.1f;
    public int maxJumpCount = 2;
    public int currentJumpCount = 0;
    public float jumpCool = 0.3f;
    public float jumpCoolTimer = 0f;
    public float hp = 100f;
    public float gravity = 9.81f;
    public int PlayerID;

    public Rigidbody2D rb;
    public SpriteRenderer sr;
    public bool isGrounded = true;
    public bool canMove = true;
    public bool isInvincible = false;

    [Header("Attacks")]
    public DashAttack dashAttack;
    public SideSmash sideSmash;
    public UpSmash upSmash;


    protected void Init(string name)
    {
        characterName = name;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        rb.linearDamping = 0.5f;

        dashAttack.Init(this);
        sideSmash.Init(this);
        upSmash.Init(this);
    }

    protected virtual void Update()
    {
        ApplyGravity();
        HandleInput();


        OnDashAttack();
        OnSideSmash();
        OnUpSmash();


        if (jumpCoolTimer > 0f)
        {
            jumpCoolTimer += Time.deltaTime;
            if (jumpCoolTimer >= jumpCool)
            {
                jumpCoolTimer = 0f;
            }
        }
        if (jumpTimer > 0f)
        {
            jumpTimer += Time.deltaTime;
            if (jumpTimer >= jumpLag)
            {
                Jump();
                jumpTimer = 0f;
            }
        }
    }

    public virtual void Move(float direction, float speedScaler)
    {
       rb.linearVelocity = new Vector2(direction * speedScaler, rb.linearVelocity.y);
    }

    public virtual void Jump()
    {
        if (currentJumpCount < maxJumpCount && jumpCoolTimer <= 0f && canMove)
        {
            jumpCoolTimer += Time.deltaTime;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
            isGrounded = false;
            currentJumpCount++;
        }
    }

    void OnDashAttack()
    {
        if (isGrounded == true && canMove == true && dashTimer >= 0.1f)
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
    }
    void OnSideSmash()
    {
        if (isGrounded == true && canMove == true && dashTimer <= 0.2f && moveDir != 0)
        {
            if (PlayerID == 1 && Input.GetKeyDown(KeyCode.Space))
            {
                sideSmash.Activate();
            }
            else if (PlayerID == 2 && Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                sideSmash.Activate();
            }
        }
    }
    void OnUpSmash()
    {
        if (isGrounded == true && canMove == true && moveDir == 0 && jumpTimer > 0f)
        {
            if (PlayerID == 1 && Input.GetKeyDown(KeyCode.Space))
            {
                upSmash.Activate();
                jumpTimer = 0f;
            }
            else if (PlayerID == 2 && Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                upSmash.Activate();
                jumpTimer = 0f;
            }
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
            currentJumpCount = 0;
        }
    }

    protected virtual void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.contacts.Length > 0 && collision.contacts[0].normal.y > 0.5f)
        {
            isGrounded = false;
            if (currentJumpCount == 0)
            {
                currentJumpCount = 1;
            }
        }
    }

    protected void HandleInput()
    {
        moveDir = 0f;

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
            if (Input.GetKeyDown(KeyCode.W)) jumpTimer += Time.deltaTime;
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
            if (Input.GetKeyDown(KeyCode.UpArrow)) jumpTimer += Time.deltaTime;
        }

        dashTimer += Time.deltaTime;
        dashTimer *= Mathf.Abs(moveDir);
        if (canMove)
        {
            Move(moveDir, speed);
        }
    }
}
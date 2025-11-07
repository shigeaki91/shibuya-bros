using System;
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
    public float jumpElapsed = 0f;
    public float jumpLag = 0.1f;
    public float jumpTimer = 0f;
    public float jumpDuration = 0.2f;
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
    public bool isJumping = false;

    [Header("Attacks")]
    public WeakAttack weakAttack;
    public DashAttack dashAttack;
    public SideSmash sideSmash;
    public UpSmash upSmash;
    public AirNeutral airNeutral;
    public AirSide airSide;
    public AirUp airUp;
    public AirDown airDown;


    protected void Init(string name)
    {
        characterName = name;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        rb.linearDamping = 0.5f;

        dashAttack.Init(this);
        sideSmash.Init(this);
        upSmash.Init(this);
        weakAttack.Init(this);
        airNeutral.Init(this);
        airSide.Init(this);
        airUp.Init(this);
        airDown.Init(this);
    }

    protected virtual void Update()
    {
        ApplyGravity();
        HandleInput();


        OnDashAttack();
        OnSideSmash();
        OnUpSmash();
        OnWeakAttack();
        OnAirNeutral();
        OnAirSide();
        OnAirUp();
        OnAirDown();


        if (jumpCoolTimer > 0f)
        {
            jumpCoolTimer += Time.deltaTime;
            if (jumpCoolTimer >= jumpCool)
            {
                jumpCoolTimer = 0f;
            }
        }
        if (jumpElapsed > 0f)
        {
            jumpElapsed += Time.deltaTime;
            if (jumpElapsed >= jumpLag)
            {
                Jump();
                jumpElapsed = 0f;
            }
        }
        if (isJumping) rb.linearVelocityY = jumpPower;
        if (jumpTimer > 0f) jumpTimer += Time.deltaTime;
        if (jumpTimer >= jumpDuration || isGrounded)
        {
            isJumping = false;
            jumpTimer = 0f;
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
            jumpTimer += Time.deltaTime;
            isJumping = true;
            isGrounded = false;
            currentJumpCount++;
        }
    }
    void OnWeakAttack()
    {
        if (isGrounded == true && canMove == true && Mathf.Abs(moveDir) < 1f)
        {
            if (PlayerID == 1 && Input.GetKeyDown(KeyCode.Space))
            {
                weakAttack.Activate();
            }
            else if (PlayerID == 2 && Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                weakAttack.Activate();
            }
        }
        if (weakAttack.isActive == true)
        {
            if (PlayerID == 1 && Input.GetKeyDown(KeyCode.Space) && weakAttack.attackCount < 3)
            {
                weakAttack.attackCount += 1;
            }
            else if (PlayerID == 2 && Input.GetKeyDown(KeyCode.KeypadEnter) && weakAttack.attackCount < 3)
            {
                weakAttack.attackCount += 1;
            }
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
        if (isGrounded == true && canMove == true && dashTimer <= 0.1f && Mathf.Abs(moveDir) == 1f)
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
        if (isGrounded == true && canMove == true && moveDir == 0 && jumpElapsed > 0f)
        {
            if (PlayerID == 1 && Input.GetKeyDown(KeyCode.Space))
            {
                upSmash.Activate();
                jumpElapsed = 0f;
            }
            else if (PlayerID == 2 && Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                upSmash.Activate();
                jumpElapsed = 0f;
            }
        }
    }
    void OnAirNeutral()
    {
        if (isGrounded == false && canMove == true && Mathf.Abs(moveDir) < 1f)
        {
            if (PlayerID == 1 && Input.GetKeyDown(KeyCode.Space) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
            {
                airNeutral.Activate();
            }
            else if (PlayerID == 2 && Input.GetKeyDown(KeyCode.KeypadEnter) && !Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow))
            {
                airNeutral.Activate();
            }
        }
    }
    void OnAirSide()
    {
        if (isGrounded == false && canMove == true && Mathf.Abs(moveDir) == 1f)
        {
            if (PlayerID == 1 && Input.GetKeyDown(KeyCode.Space) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
            {
                airSide.Activate();
            }
            else if (PlayerID == 2 && Input.GetKeyDown(KeyCode.KeypadEnter) && !Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow))
            {
                airSide.Activate();
            }
        }
    }
    void OnAirUp()
    {
        if (isGrounded == false && canMove == true)
        {
            if (PlayerID == 1 && Input.GetKeyDown(KeyCode.Space) && Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
            {
                airUp.Activate();
            }
            else if (PlayerID == 2 && Input.GetKeyDown(KeyCode.KeypadEnter) && Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow))
            {
                airUp.Activate();
            }
        }
    }
    void OnAirDown()
    {
        if (isGrounded == false && canMove == true)
        {
            if (PlayerID == 1 && Input.GetKeyDown(KeyCode.Space) && Input.GetKey(KeyCode.S))
            {
                airDown.Activate();
            }
            else if (PlayerID == 2 && Input.GetKeyDown(KeyCode.KeypadEnter) && Input.GetKey(KeyCode.DownArrow))
            {
                airDown.Activate();
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
        if (moveDir < 0f) moveDir += Time.deltaTime * 5f;
        else if (moveDir > 0f) moveDir -= Time.deltaTime * 5f;
        if (Mathf.Abs(moveDir) < 0.1f) moveDir = 0f;

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
            if (Input.GetKeyDown(KeyCode.W)) jumpElapsed += Time.deltaTime;
            if (!Input.GetKey(KeyCode.W)) isJumping = false;
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
            if (Input.GetKeyDown(KeyCode.UpArrow)) jumpElapsed += Time.deltaTime;
            if (!Input.GetKey(KeyCode.UpArrow)) isJumping = false;
        }

        dashTimer += Time.deltaTime;
        if (Mathf.Abs(moveDir) < 1f) dashTimer = 0f;
        if (canMove)
        {
            Move(moveDir, speed);
        }
    }
}
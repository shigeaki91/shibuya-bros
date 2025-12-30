using System;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Scripting.APIUpdating;

public abstract class Character : MonoBehaviour
{
    public string characterName;
    public float speed = 5f;
    public float moveDir = 0f;
    public float dashTimer = 0f;
    public float jumpPower = 10f;
    public float jumpElapsed = 0f;
    public float jumpLagTime = 0.1f;
    public float jumpHoldTimer = 0f;
    public float jumpHoldTime = 0.2f;
    public int maxJumpCount = 2;
    public int currentJumpCount = 0;
    public float jumpCoolTime = 0.3f;
    public float jumpCoolTimer = 0f;
    public float hp = 100f;
    public float gravity = 9.81f;
    public int PlayerID;
    public InputActionAsset _inputAction;
    public InputActionMap _inputActionMap;

    public Rigidbody2D rb;
    public SpriteRenderer sr;
    public bool isGrounded = true;
    public bool canMove = true;
    public bool isInvincible = false;
    public bool isJumpHolding = false;
    protected void Init(string name)
    {
        characterName = name;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        rb.linearDamping = 0.5f;

        _inputActionMap = _inputAction.FindActionMap($"Player{PlayerID}");
        _inputActionMap.Enable();
    }

    protected virtual void Update()
    {
        ApplyGravity();
        HandleMovement();
        HandleJump();
    }

    public virtual void Move(float direction, float speedScaler)
    {
       rb.linearVelocity = new Vector2(direction * speedScaler, rb.linearVelocity.y);
    }

    public virtual void Jump()
    {
        if (currentJumpCount < maxJumpCount && jumpCoolTimer == 0f && canMove)
        {
            jumpCoolTimer += Time.deltaTime;
            jumpHoldTimer += Time.deltaTime;
            isJumpHolding = true;
            isGrounded = false;
            currentJumpCount++;
        }
    }

    public void SetPlayerID(int id)
    {
        PlayerID = id;
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

    protected void HandleMovement()
    {
        if (moveDir < 0f) moveDir += Time.deltaTime * 5f;
        else if (moveDir > 0f) moveDir -= Time.deltaTime * 5f;
        if (Mathf.Abs(moveDir) < 0.1f) moveDir = 0f;

        moveDir = _inputActionMap.FindAction("Move").ReadValue<float>();
        if (moveDir < 0f) sr.flipX = true;
        else if (moveDir > 0f) sr.flipX = false;

        dashTimer += Time.deltaTime;
        if (Mathf.Abs(moveDir) < 1f) dashTimer = 0f;

        if (canMove)
        {
            Move(moveDir, speed);
        }
    }

    protected void HandleJump()
    {
        _inputActionMap.FindAction("Jump").performed += ctx =>
        {
            jumpElapsed += Time.deltaTime;
            Debug.Log("Jump Pressed");
        };

        if (jumpCoolTimer > 0f)
        {
            jumpCoolTimer += Time.deltaTime;
            if (jumpCoolTimer >= jumpCoolTime)
            {
                jumpCoolTimer = 0f;
            }
        }
        if (jumpElapsed > 0f)
        {
            jumpElapsed += Time.deltaTime;
            if (jumpElapsed >= jumpLagTime)
            {
                Jump();
                jumpElapsed = 0f;
            }
        }
        if (isJumpHolding) rb.linearVelocityY = jumpPower; //実質的なジャンプ処理はここ。
        if (jumpHoldTimer > 0f) jumpHoldTimer += Time.deltaTime;
        if (jumpHoldTimer >= jumpHoldTime || isGrounded || !_inputActionMap.FindAction("Jump").IsPressed())
        {
            isJumpHolding = false;
            jumpHoldTimer = 0f;
        }
    }


    public AttackTypes GetAttackState()
    {
        if (canMove)
        {
            if (isGrounded) //地上
            {
                if (Mathf.Abs(moveDir) < 1f) 
                {
                    if (jumpElapsed > 0f)
                    {
                        return AttackTypes.UpSmash;
                    }
                    else
                    {
                        return AttackTypes.WeakAttack;
                    }
                }
                else if (Mathf.Abs(moveDir) == 1f)
                {
                    if (dashTimer <= 0.1f)
                    {
                        return AttackTypes.SideSmash;
                    }
                    else
                    {
                        return AttackTypes.DashAttack;
                    }
                }
                else
                {
                    return AttackTypes.WeakAttack;
                }
            }
            else //空中
            {
                if (Mathf.Abs(moveDir) < 1f)
                {
                    if ((PlayerID == 1 && Input.GetKey(KeyCode.W)) || (PlayerID == 2 && Input.GetKey(KeyCode.UpArrow)))
                    {
                        return AttackTypes.AirUp;
                    }
                    
                    if ((PlayerID == 1 && Input.GetKey(KeyCode.S)) || (PlayerID == 2 && Input.GetKey(KeyCode.DownArrow)))
                    {
                        return AttackTypes.AirDown;
                    }
                    return AttackTypes.AirNeutral;
                }
                else if (Mathf.Abs(moveDir) == 1f)
                {
                    return AttackTypes.AirSide;
                    
                }
                else
                {
                    return AttackTypes.AirNeutral;
                }
            }
        }
        else
        {
            return AttackTypes.None;
        }
    }
}


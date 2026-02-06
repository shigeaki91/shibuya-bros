using UnityEngine;
using UnityEngine.InputSystem;
using R3;
using Extensions;
using System.Collections;

public abstract class Character : MonoBehaviour
{
    public CharacterNames characterName;
    public float speed = 7f;
    float moveDir = 0f;
    float dashTimer = 0f;
    float jumpPower = 7f;
    float jumpElapsed = 0f;
    float jumpLagTime = 0.1f;
    float jumpHoldTimer = 0f;
    float jumpHoldTime = 0.2f;
    int maxJumpCount = 2;
    int currentJumpCount = 0;
    float jumpCoolTime = 0.3f;
    float jumpCoolTimer = 0f;
    float _maxHp = 100f;
    public ReactiveProperty<float> hp = new ReactiveProperty<float>(100f);
    float gravity = 9.81f;
    public int PlayerID;
    [SerializeField] InputActionAsset _inputAction;
    InputActionMap _inputActionMap;
    public Animator Animator;


    public Rigidbody2D rb;
    public SpriteRenderer sr;
    [SerializeField] SpriteRenderer faceSr;
    public bool isGrounded = true;
    public bool isAttacking = false;
    public bool isTakingDamage = false;
    public bool isInvincible = false;
    public bool isStarting;
    bool isJumpHolding = false;
    float SpecialDurationTimer = 0f;
    float _specialDurationTime = 15f;
    float _specialChargeTime = 1.5f;
    TMPro.TMP_Text _playerIndexText;
    
    protected void Init(CharacterNames name)
    {
        isStarting = true;
        characterName = name;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        Animator = GetComponent<Animator>();
        _playerIndexText = GetComponentInChildren<TMPro.TMP_Text>();
        _playerIndexText.text = $"P{PlayerID}▼";
        _playerIndexText.color = PlayerID == 1 ? Color.red : Color.blue;
        _playerIndexText.fontSize = 30;
        rb.linearDamping = 1f;
        hp.Value = _maxHp;
        sr.sortingOrder = PlayerID*2;
        faceSr.sortingOrder = PlayerID*2 + 1;

        _inputActionMap = _inputAction.FindActionMap($"Player{PlayerID}");
        _inputActionMap.Enable();

        SPInit();
    }

    public void SPInit()
    {
        Debug.Log("Special Attack Initialized for " + characterName);
        var specialChargeObservable = ObservableEx.ChargeActionByObservable(_inputActionMap.FindAction("Attack"), _specialChargeTime);
        specialChargeObservable
            .Where(_ => CanSpecialAttack())
            .Select(charge => charge >= 1.0f)
            .DistinctUntilChanged()
            .Subscribe(_ =>
            {
                SPActivate();
            }).AddTo(this);
    }

    public virtual void SPActivate()
    {
        SpecialDurationTimer = 0f;
        isAttacking = true;
        Animator.SetBool("Idling", false);
        //Debug.Log($"{characterName} Special Attack Activated!");
    }

    public virtual void SPDeactivate()
    {
        Animator.SetBool("Idling", true);
        isAttacking = false;
        //Debug.Log($"{characterName} Special Attack ended");
    }

    public float GetSpecialChargeRatio()
    {
        return Mathf.Clamp01(SpecialDurationTimer / _specialDurationTime);
    }

    protected virtual void Update()
    {
        ApplyGravity();
        HandleMovement();
        HandleJump();
        AirAnimationControl();
        KnockBackControl();
        if (!isStarting)
        {
            if (SpecialDurationTimer < _specialDurationTime)
            {
                SpecialDurationTimer += Time.deltaTime;
            }
            else if (SpecialDurationTimer >= _specialDurationTime && isAttacking)
            {
                SpecialDurationTimer = _specialDurationTime;
            }
        }
        
    }

    public virtual void Move(float direction, float speedScaler)
    {
       rb.linearVelocity = new Vector2(direction * speedScaler, rb.linearVelocity.y);
    }

    public virtual void Jump()
    {
        if (CanJump())
        {
            jumpCoolTimer += Time.deltaTime;
            jumpHoldTimer += Time.deltaTime;
            isJumpHolding = true;
            isGrounded = false;
            currentJumpCount++;
            Animator.SetTrigger("Jump");
            Animator.ResetTrigger("Landing");
            AudioManager.Instance.PlaySFX(SFXtypes.Jump);
        }
    }

    public bool CanMove()
    {
        return !isStarting &&!isAttacking && !isTakingDamage || !isGrounded;
    }
    
    public bool CanJump()
    {
        return !isStarting && !isAttacking && !isTakingDamage && currentJumpCount < maxJumpCount && jumpCoolTimer == 0f;
    }

    public bool CanAttack()
    {
        return !isStarting && !isAttacking && !isTakingDamage;
    }

    public bool CanSpecialAttack()
    {
        return !isStarting && !isAttacking && !isTakingDamage && SpecialDurationTimer >= _specialDurationTime;
    }

    public void SetPlayerID(int id)
    {
        PlayerID = id;
    }

    public IEnumerator TakeDamage(float damage)
    {
        hp.Value -= damage;
        isTakingDamage = true;
        isGrounded = false;
        Animator.ResetTrigger("Landing");
        if (damage > 8f) 
        {
            SpecialDurationTimer += _specialDurationTime * 0.05f;
            Animator.SetTrigger("KnockBack1");
            AudioManager.Instance.PlaySFX(SFXtypes.HitHeavy);
            if (hp.Value > 0f)
            {
                Time.timeScale = 0f;
                yield return new WaitForSecondsRealtime(0.08f);
                Time.timeScale = 1f;
            }
        }
        else 
        {
            SpecialDurationTimer += _specialDurationTime * 0.02f;
            Animator.SetTrigger("KnockBack2");
            AudioManager.Instance.PlaySFX(SFXtypes.HitLight);
            yield return null;
        }
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
        if (collision.contacts[0].normal.y > 0.5f && collision.gameObject.CompareTag("Ground"))
        {
            currentJumpCount = 0;
            Animator.SetTrigger("Landing");
            Animator.ResetTrigger("Jump");

            if (isTakingDamage)
            {
                rb.linearVelocityY = rb.linearVelocityY * -0.5f;
            }
        }
    }

    protected virtual void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0.5f && collision.gameObject.CompareTag("Ground") && !isGrounded && rb.linearVelocityY <= 0f)
        {
            isGrounded = true;
        }
    }

    protected virtual void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.contacts.Length > 0 && collision.contacts[0].normal.y > 0.5f && collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            if (currentJumpCount == 0)
            {
                currentJumpCount = 1;
            }
        }
    }

    void AirAnimationControl()
    {
        if (!isGrounded)
        {
            if (rb.linearVelocity.y > 0f)
            {
                Animator.SetBool("JumpUp", true);
                Animator.SetBool("JumpDown", false);
            }
            else if (rb.linearVelocity.y < 0f)
            {
                Animator.SetBool("JumpUp", false);
                Animator.SetBool("JumpDown", true);
            }
        }
        else
        {
            Animator.SetBool("JumpUp", false);
            Animator.SetBool("JumpDown", false);
            Animator.SetTrigger("Landing");
        }
    }

    void KnockBackControl()
    {
        if (isTakingDamage)
        {
            Animator.SetBool("Down", true);
        }
        else
        {
            Animator.SetBool("Down", false);
        }

        if (Animator.GetCurrentAnimatorStateInfo(0).IsName("Down") && Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            Animator.speed = 0f;
        }
        else
        {
            Animator.speed = 1f;
        }
    }

    protected void HandleMovement()
    {
        if (moveDir < 0f) moveDir += Time.deltaTime * 5f;
        else if (moveDir > 0f) moveDir -= Time.deltaTime * 5f;
        if (Mathf.Abs(moveDir) < 0.1f) moveDir = 0f;

        if (CanMove()) moveDir = _inputActionMap.FindAction("Move").ReadValue<float>() == 0 ? moveDir : _inputActionMap.FindAction("Move").ReadValue<float>();
        if (moveDir < 0f) sr.flipX = true;
        else if (moveDir > 0f) sr.flipX = false;

        dashTimer += Time.deltaTime;
        if (Mathf.Abs(moveDir) < 1f) dashTimer = 0f;

        if (!isTakingDamage) Move(moveDir, speed);

        Animator.SetBool("Running", Mathf.Abs(moveDir) == 1f);
        Animator.SetBool("Idling", Mathf.Abs(moveDir) < 1f);
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

    public float GetDownTimeMultiplier()
    {
        return Mathf.Sqrt(1f + (0.02f * (_maxHp - hp.Value)));
    }

    public float GetHitKnockbackMultiplier()
    {
        return Mathf.Sqrt(1f + (0.01f * (_maxHp - hp.Value)));
    }

    public AttackTypes GetAttackState()
    {
        if (CanAttack())
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
                if (_inputActionMap.FindAction("Down").IsPressed())
                {
                    return AttackTypes.AirDown;
                }
                else if (_inputActionMap.FindAction("Up").IsPressed())
                {
                    return AttackTypes.AirUp;
                }
                else if (Mathf.Abs(moveDir) < 1f)
                {
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


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

    protected Rigidbody2D rb;
    protected bool isGrounded = true;

    protected void Init(string name)
    {
        characterName = name;
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        ApplyGravity();
    }

    public virtual void Move(float direction)
    {
       rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);
    }
    
    public virtual void Jump()
    {
        if (isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
            isGrounded = false;
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
            if (Input.GetKey(KeyCode.A)) moveDir = -1f;
            else if (Input.GetKey(KeyCode.D)) moveDir = 1f;
            if (Input.GetKeyDown(KeyCode.W)) Jump();
        }
        else if (PlayerID == 2)
        {
            if (Input.GetKey(KeyCode.LeftArrow)) moveDir = -1f;
            else if (Input.GetKey(KeyCode.RightArrow)) moveDir = 1f;
            if (Input.GetKeyDown(KeyCode.UpArrow)) Jump();
        }
        Move(moveDir);
    }
}
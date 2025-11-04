using System.Timers;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public Character owner;
    public float damage;
    public Vector2 knockback;
    public float InvincibilityDuration = 0.5f;
    public float downTime = 0.2f;
    private Rigidbody2D rb;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Character target = collision.GetComponent<Character>();
        if (target != null && target != owner && target.isInvincible == false)
        {
            target.canMove = false;
            target.isInvincible = true;
            //target.StartCoroutine(DownCoroutine(target, downTime));
            //target.StartCoroutine(InvincibilityCoroutine(target, InvincibilityDuration));
            target.TakeDamage(damage);
            rb = target.GetComponent<Rigidbody2D>();
            rb.linearVelocity = knockback;
        }
    }

    private System.Collections.IEnumerator DownCoroutine(Character target, float downTime)
    {
        float elapsed = 0f;
        while (elapsed < downTime)
        {
            elapsed += Time.deltaTime;
            rb.linearVelocityX -= (knockback.x / downTime) * Time.deltaTime;
            yield return null;
        }
        target.canMove = true;
    }
    private System.Collections.IEnumerator InvincibilityCoroutine(Character target, float duration)
    {
        float elapsed = 0f;
        float blinkInterval = 0.2f;
        SpriteRenderer sr = target.sr;

        while (elapsed < duration)
        {
            sr.enabled = !sr.enabled;
            yield return new WaitForSeconds(blinkInterval);
            elapsed += blinkInterval;
        }
        sr.enabled = true;
        target.isInvincible = false;
    }

}
using System.Timers;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public Character owner;
    public float damage;
    public Vector2 knockback;
    public float InvincibilityDuration = 0.05f;
    public float downTime = 0.35f;
    public bool hit = false;
    private Rigidbody2D rb;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Character target = collision.GetComponent<Character>();
        if (target != null && target != owner && target.isInvincible == false)
        {
            hit = true;
            target.canMove = false;
            //target.isInvincible = true;
            target.StartCoroutine(DownCoroutine(target, downTime));
            target.StartCoroutine(InvincibilityCoroutine(target, InvincibilityDuration));
            target.TakeDamage(damage);
            rb = target.GetComponent<Rigidbody2D>();
            rb.linearVelocity = knockback;
        }
    }

    private System.Collections.IEnumerator DownCoroutine(Character target, float downTime)
    {
        yield return new WaitForSeconds(downTime);
        target.canMove = true;
    }
    private System.Collections.IEnumerator InvincibilityCoroutine(Character target, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        target.isInvincible = false;
    }

}
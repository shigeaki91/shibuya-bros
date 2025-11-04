using UnityEngine;

public class DashAttack : Attack
{
    public float dashSpeed;
    public HitBox hitBox;
    void Start()
    {
        Init(owner);
        attackName = "Dash Attack";
        damage = 15f;
        knockback = new Vector2(2f, 7f);
        duration = 0.5f;
        hitBox.gameObject.SetActive(false);
    }

    public override void Activate()
    {
        base.Activate();
        dashSpeed = owner.speed * 3f;

        hitBox.owner = owner;
        hitBox.damage = damage;
        hitBox.knockback = knockback;
        hitBox.gameObject.SetActive(true);

        float direction = owner.sr.flipX ? -1f : 1f;
        hitBox.knockback.x = knockback.x * direction;

        Vector2 localPos = hitBox.transform.localPosition;
        localPos.x = Mathf.Abs(localPos.x) * direction;
        hitBox.transform.localPosition = localPos;
        
        owner.StartCoroutine(DashCoroutine(direction));
    }

    private System.Collections.IEnumerator DashCoroutine(float direction)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            owner.Move(direction, dashSpeed);
            elapsed += Time.deltaTime;
            dashSpeed -= (owner.speed * 3f / duration) * Time.deltaTime;
            yield return null;
        }

        hitBox.gameObject.SetActive(false);
        Deactivate();
    }
}

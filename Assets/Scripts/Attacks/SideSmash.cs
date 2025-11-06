using UnityEngine;

public class SideSmash : Attack
{
    public HitBox hitBox;
    void Start()
    {
        attackName = "Side Smash";
        damage = 15.2f;
        knockback = new Vector2(12f, 5f);
        occurTime = 0.5f;
        duration = 0.5f;
        endingLag = 0.3f;

        hitBox.owner = owner;
        hitBox.damage = damage;
        hitBox.gameObject.SetActive(false);
    }

    public override void Activate()
    {
        base.Activate();
        hitBox.knockback = knockback;
        hitBox.knockback.x = knockback.x * direction;
        Vector2 localPos = hitBox.transform.localPosition;
        localPos.x = Mathf.Abs(localPos.x) * direction;
        hitBox.transform.localPosition = localPos;

        owner.StartCoroutine(SideSmashCoroutine());
    }

    private System.Collections.IEnumerator SideSmashCoroutine()
    {
        owner.rb.linearVelocityX = 0f;
        yield return new WaitForSeconds(occurTime);
        hitBox.gameObject.SetActive(true);
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(endingLag);
        hitBox.gameObject.SetActive(false);
        Deactivate();
    }
}

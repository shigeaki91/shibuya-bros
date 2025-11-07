using UnityEngine;

public class SideSmash : Attack
{
    public HitBox hitBox;
    void Start()
    {
        attackName = "Side Smash";
        damage[0] = 15.2f;
        knockback[0] = new Vector2(12f, 5f);
        occurTime = 0.5f;
        duration = 0.5f;
        endingLag = 0.3f;

        hitBox.owner = owner;
        hitBox.damage = damage[0];
        hitBox.gameObject.SetActive(false);
    }

    public override void Activate()
    {
        base.Activate();
        hitBox.knockback = knockback[0];
        hitBox.knockback.x = knockback[0].x * direction;
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
        hitBox.gameObject.SetActive(false);
        yield return new WaitForSeconds(endingLag);
        Deactivate();
    }
}

using UnityEngine;

public class UpSmash : Attack
{
    public HitBox hitBox;
    void Start()
    {
        attackName = "Up Smash";
        damage = 12.0f;
        knockback = new Vector2(0f, 10f);
        occurTime = 0.4f;
        duration = 0.4f;
        endingLag = 0.3f;

        hitBox.owner = owner;
        hitBox.damage = damage;
        hitBox.gameObject.SetActive(false);
    }

    public override void Activate()
    {
        base.Activate();
        hitBox.knockback = knockback;
        Vector2 localPos = hitBox.transform.localPosition;
        localPos.x = Mathf.Abs(localPos.x) * direction;
        hitBox.transform.localPosition = localPos;

        owner.StartCoroutine(UpSmashCoroutine());
    }

    private System.Collections.IEnumerator UpSmashCoroutine()
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

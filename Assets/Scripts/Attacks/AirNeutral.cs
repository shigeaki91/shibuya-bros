using UnityEngine;

public class AirNeutral : Attack
{
    public HitBox hitBox;
    void Start()
    {
        attackName = "Air Neutral";
        damage[0] = 4.6f;
        knockback[0] = new Vector2(3f, 2f);
        occurTime = 0.1f;
        duration = 0.6f;
        endingLag = 0.15f;
        
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
        
        owner.StartCoroutine(DashCoroutine(direction));
    }

    private System.Collections.IEnumerator DashCoroutine(float direction)
    {
        yield return new WaitForSeconds(occurTime);
        float elapsed = 0f;
        hitBox.gameObject.SetActive(true);
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            yield return null;
            if (owner.isGrounded)
            {
                break;
            }
        }
        hitBox.gameObject.SetActive(false);
        yield return new WaitForSeconds(endingLag);
        Deactivate();
    }
}

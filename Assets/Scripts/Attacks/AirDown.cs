using UnityEngine;

public class AirDown : Attack
{
    public HitBox hitBox;
    void Start()
    {
        attackName = "Air Down";
        damage[0] = 8.2f;
        knockback[0] = new Vector2(0f, -5f);
        occurTime = 0.2f;
        duration = 0.2f;
        endingLag = 0.4f;
        
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

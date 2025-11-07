using UnityEngine;

public class DashAttack : Attack
{
    public float dashSpeed;
    public HitBox hitBox;
    void Start()
    {
        attackName = "Dash Attack";
        damage[0] = 6.4f;
        knockback[0] = new Vector2(2f, 4f);
        occurTime = 0.15f;
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

        dashSpeed = owner.speed * 3f;
        
        owner.StartCoroutine(DashCoroutine(direction));
    }

    private System.Collections.IEnumerator DashCoroutine(float direction)
    {
        yield return new WaitForSeconds(occurTime);
        float elapsed = 0f;
        hitBox.gameObject.SetActive(true);
        while (elapsed < duration)
        {
            owner.Move(direction, dashSpeed);
            elapsed += Time.deltaTime;
            dashSpeed -= (owner.speed * 3f / duration) * Time.deltaTime;
            yield return null;
        }
        hitBox.gameObject.SetActive(false);
        yield return new WaitForSeconds(endingLag);
        Deactivate();
    }
}

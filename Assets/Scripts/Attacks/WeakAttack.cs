using System.Collections.Generic;
using UnityEngine;

public class WeakAttack : Attack
{
    public int attackCount = 0;
    public List<HitBox> hitBox;
    void Start()
    {
        attackName = "Weak Attack";
        damage[0] = 2.0f;
        damage[1] = 2.4f;
        damage[2] = 3.6f;
        knockback[0] = new Vector2(0.2f, 2f);
        knockback[1] = new Vector2(0.4f, 2f);
        knockback[2] = new Vector2(4f, 2f);

        occurTime = 0.05f;
        duration = 0.1f;
        endingLag = 0.2f;
        for (int i = 0; i < 3; i++)
        {
            hitBox[i].owner = owner;
            hitBox[i].damage = damage[i];
            hitBox[i].gameObject.SetActive(false);
        }
    }

    public override void Activate()
    {
        base.Activate();
        for (int i = 0; i < 3; i++)
        {
            hitBox[i].knockback = knockback[i];
            hitBox[i].knockback.x = knockback[i].x * direction;
            Vector2 localPos = hitBox[i].transform.localPosition;
            localPos.x = Mathf.Abs(localPos.x) * direction;
            hitBox[i].transform.localPosition = localPos;
        }

        owner.StartCoroutine(WeakAttackCoroutine0());
    }

    private System.Collections.IEnumerator WeakAttackCoroutine0()
    {
        owner.rb.linearVelocityX = 0f;
        yield return new WaitForSeconds(occurTime);
        hitBox[0].gameObject.SetActive(true);
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
        hitBox[0].gameObject.SetActive(false);
        yield return new WaitForSeconds(endingLag);
        if (attackCount >= 2 && hitBox[0].hit == true)
        {
            owner.StartCoroutine(WeakAttackCoroutine1());
            hitBox[0].hit = false;
        }
        else
        {
            Deactivate();
            attackCount = 0;
        }
    }

    private System.Collections.IEnumerator WeakAttackCoroutine1()
    {
        owner.rb.linearVelocityX = 0f;
        yield return new WaitForSeconds(occurTime);
        hitBox[1].gameObject.SetActive(true);
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
        hitBox[1].gameObject.SetActive(false);
        yield return new WaitForSeconds(endingLag);
        if (attackCount == 3) owner.StartCoroutine(WeakAttackCoroutine2());
        else
        {
            Deactivate();
            attackCount = 0;
        }
    }

    private System.Collections.IEnumerator WeakAttackCoroutine2()
    {
        owner.rb.linearVelocityX = 0f;
        yield return new WaitForSeconds(occurTime);
        hitBox[2].gameObject.SetActive(true);
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
        hitBox[2].gameObject.SetActive(false);
        yield return new WaitForSeconds(endingLag);
        Deactivate();
        attackCount = 0;
    }
}

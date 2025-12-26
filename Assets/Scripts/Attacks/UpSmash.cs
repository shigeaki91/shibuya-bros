using UnityEngine;
using R3;

public class UpSmash : Attack
{
    public HitBox hitBox;
    public UpSmash(Character owner, Observable<Unit> attackInput, UpSmashConfig config)
    {
        _attackName = config.AttackName;
        _damage[0] = config.Damage;
        _knockback[0] = config.Knockback;
        _occurTime = config.OccurTime;
        _duration = config.Duration;
        _endingLag = config.EndingLag;
        _attackInput = attackInput;

        hitBox.owner = owner;
        hitBox.damage = _damage[0];
        hitBox.gameObject.SetActive(false);
    }

    public override void Activate()
    {
        base.Activate();
        hitBox.knockback = _knockback[0];
        Vector2 localPos = hitBox.transform.localPosition;
        localPos.x = Mathf.Abs(localPos.x) * _direction;
        hitBox.transform.localPosition = localPos;

        owner.StartCoroutine(UpSmashCoroutine());
    }

    private System.Collections.IEnumerator UpSmashCoroutine()
    {
        owner.rb.linearVelocityX = 0f;
        yield return new WaitForSeconds(_occurTime);
        hitBox.gameObject.SetActive(true);
        float elapsed = 0f;
        while (elapsed < _duration)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
        hitBox.gameObject.SetActive(false);
        yield return new WaitForSeconds(_endingLag);
        Deactivate();
    }
}

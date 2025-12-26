using UnityEngine;
using R3;

public class AirUp : Attack
{
    public HitBox hitBox;
    public AirUp(Character owner, Observable<Unit> attackInput, AirUpConfig config)
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
        hitBox.knockback.x = _knockback[0].x * _direction;
        Vector2 localPos = hitBox.transform.localPosition;
        localPos.x = Mathf.Abs(localPos.x) * _direction;
        hitBox.transform.localPosition = localPos;
        
        owner.StartCoroutine(DashCoroutine(_direction));
    }

    private System.Collections.IEnumerator DashCoroutine(float direction)
    {
        yield return new WaitForSeconds(_occurTime);
        float elapsed = 0f;
        hitBox.gameObject.SetActive(true);
        while (elapsed < _duration)
        {
            elapsed += Time.deltaTime;
            yield return null;
            if (owner.isGrounded)
            {
                break;
            }
        }
        hitBox.gameObject.SetActive(false);
        yield return new WaitForSeconds(_endingLag);
        Deactivate();
    }
}

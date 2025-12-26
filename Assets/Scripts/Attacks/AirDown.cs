using UnityEngine;
using UnityEngine.InputSystem;
using R3;

public class AirDown : Attack
{
    public HitBox _hitBox;
    Rigidbody2D _rb;
    public AirDown(Character owner, Observable<Unit> attackInput, AirDownConfig config)
    {
        Init(owner);
        _attackName = config.AttackName;
        _damage[0] = config.Damage;
        _knockback[0] = config.Knockback;
        _occurTime = config.OccurTime;
        _duration = config.Duration;
        _endingLag = config.EndingLag;
        _attackInput = attackInput;
        
        _hitBox.owner = owner;
        _hitBox.damage = _damage[0];
        _hitBox.gameObject.SetActive(false);
        _rb = owner.rb;

        _attackInput
            .Where(_ => !owner.isGrounded && owner.canMove)
            .
    }

    public override void Activate()
    {
        base.Activate();
        _hitBox.knockback = _knockback[0];
        _hitBox.knockback.x = _knockback[0].x * _direction;
        Vector2 localPos = _hitBox.transform.localPosition;
        localPos.x = Mathf.Abs(localPos.x) * _direction;
        _hitBox.transform.localPosition = localPos;
        
        owner.StartCoroutine(DashCoroutine(_direction));
    }

    private System.Collections.IEnumerator DashCoroutine(float direction)
    {
        yield return new WaitForSeconds(_occurTime);
        float elapsed = 0f;
        _hitBox.gameObject.SetActive(true);
        while (elapsed < _duration)
        {
            elapsed += Time.deltaTime;
            yield return null;
            if (owner.isGrounded)
            {
                break;
            }
        }
        _hitBox.gameObject.SetActive(false);
        yield return new WaitForSeconds(_endingLag);
        Deactivate();
    }
}

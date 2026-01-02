using UnityEngine;

[CreateAssetMenu(fileName = "DashAttackConfig", menuName = "AttackConfigs/DashAttackConfig")]
public class DashAttackConfig : ScriptableObject
{
    [SerializeField] string _attackName = "Dash Attack";
    [SerializeField] float _damage = 6.4f;
    [SerializeField] Vector2 _knockback = new Vector2(2f, 4f);
    [SerializeField] float _occurTime = 0.15f;
    [SerializeField] float _duration = 0.3f;
    [SerializeField] float _endingLag = 0.05f;
    [SerializeField] float _downTime = 0.35f;

    public string AttackName => _attackName;
    public float Damage => _damage;
    public Vector2 Knockback => _knockback;
    public float OccurTime => _occurTime;
    public float Duration => _duration;
    public float EndingLag => _endingLag;
    public float DownTime => _downTime;
}
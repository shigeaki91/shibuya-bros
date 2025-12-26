using UnityEngine;

[CreateAssetMenu(fileName = "WeakAttackConfig", menuName = "AttackConfigs/WeakAttackConfig")]
public class WeakAttackConfig : ScriptableObject
{
    [SerializeField] string _attackName = "Weak Attack";
    [SerializeField] float[] _damage = new float[] { 2.0f, 2.4f, 3.6f };
    [SerializeField] Vector2[] _knockback = new Vector2[] { new Vector2(0.2f, 2f), new Vector2(0.4f, 2f), new Vector2(4f, 2f) };
    [SerializeField] float _occurTime = 0.05f;
    [SerializeField] float _duration = 0.1f;
    [SerializeField] float _endingLag = 0.2f;

    public string AttackName => _attackName;
    public float[] Damage => _damage;
    public Vector2[] Knockback => _knockback;
    public float OccurTime => _occurTime;
    public float Duration => _duration;
    public float EndingLag => _endingLag;
}
using UnityEngine;

[CreateAssetMenu(fileName = "WeakAttackConfig", menuName = "AttackConfigs/WeakAttackConfig")]
public class WeakAttackConfig : ScriptableObject
{
    [SerializeField] string _attackName = "Weak Attack";
    [SerializeField] float[] _damage = new float[] { 2.0f, 2.4f, 3.6f };
    [SerializeField] Vector2[] _knockback = new Vector2[] { new Vector2(0.2f, 2f), new Vector2(0.4f, 2f), new Vector2(4f, 2f) };
    [SerializeField] float[] _occurTime = new float[] { 0.1f, 0.1f, 0.18f };
    [SerializeField] float[] _duration = new float[] { 0.07f, 0.07f, 0.08f };
    [SerializeField] float[] _endingLag = new float[] { 0.1f, 0.1f, 0.15f };
    [SerializeField] float[] _downTime = new float[] { 0.15f, 0.15f, 0.35f };

    public string AttackName => _attackName;
    public float[] Damage => _damage;
    public Vector2[] Knockback => _knockback;
    public float[] OccurTime => _occurTime;
    public float[] Duration => _duration;
    public float[] EndingLag => _endingLag;
    public float[] DownTime => _downTime;
}
using UnityEngine;

[CreateAssetMenu(fileName = "AirDownConfig", menuName = "AttackConfigs/AirDownConfig")]
public class AirDownConfig : ScriptableObject
{
    [SerializeField] string _attackName = "Air Down";
    [SerializeField] float _damage = 8.2f;
    [SerializeField] Vector2 _knockback = new Vector2(0f, -5f);
    [SerializeField] float _occurTime = 0.35f;
    [SerializeField] float _duration = 0.12f;
    [SerializeField] float _endingLag = 0.2f;
    [SerializeField] float _downTime = 0.5f;

    public string AttackName => _attackName;
    public float Damage => _damage;
    public Vector2 Knockback => _knockback;
    public float OccurTime => _occurTime;
    public float Duration => _duration;
    public float EndingLag => _endingLag;
    public float DownTime => _downTime;
}
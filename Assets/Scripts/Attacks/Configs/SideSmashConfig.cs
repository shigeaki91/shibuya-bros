using UnityEngine;

[CreateAssetMenu(fileName = "SideSmashConfig", menuName = "AttackConfigs/SideSmashConfig")]
public class SideSmashConfig : ScriptableObject
{
    [SerializeField] string _attackName = "Side Smash";
    [SerializeField] float _damage = 15.2f;
    [SerializeField] Vector2 _knockback = new Vector2(12f, 5f);
    [SerializeField] float _occurTime = 0.5f;
    [SerializeField] float _duration = 0.5f;
    [SerializeField] float _endingLag = 0.3f;

    public string AttackName => _attackName;
    public float Damage => _damage;
    public Vector2 Knockback => _knockback;
    public float OccurTime => _occurTime;
    public float Duration => _duration;
    public float EndingLag => _endingLag;
}
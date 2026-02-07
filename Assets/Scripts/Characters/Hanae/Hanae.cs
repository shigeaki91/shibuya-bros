using UnityEngine;
using Cysharp.Threading.Tasks;

public class Hanae : Character
{
    [SerializeField] GameObject _musicNotePrefab;
    [SerializeField] int _numberOfNotes = 10;
    void Start()
    {
        Init(CharacterNames.Hanae);
        _specialLayerIndex = Animator.GetLayerIndex("Hanae Layer");
    }
    protected override void Update()
    {
        base.Update();
    }
    public override void SPActivate()
    {
        Debug.Log("Special Attack Activated for " + characterName);
        base.SPActivate();
        HanaeSP().Forget();
    }

    public override void SPDeactivate()
    {
        Debug.Log("Special Attack Deactivated for " + characterName);
        base.SPDeactivate();
    }

    async UniTask HanaeSP()
    {
        var dir = sr.flipX ? -1 : 1;
        var noteSpawnPosition = Vector2.zero;
        var angle = 0f;
        await UniTask.Delay(300);
        for (int i = 0; i < _numberOfNotes; i++)
        {
            angle = Random.Range(-5f, 30f);
            noteSpawnPosition.x = Mathf.Cos(angle * Mathf.Deg2Rad) * 1.5f * dir;
            noteSpawnPosition.y = Mathf.Sin(angle * Mathf.Deg2Rad) * 1.5f;
            var noteGo = Instantiate(_musicNotePrefab, (Vector2)transform.position + noteSpawnPosition, Quaternion.identity);
            var note = noteGo.GetComponent<HanaeMusicNotes>();
            note.Owner = this;
            note.SetSpeed(new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad) * dir, Mathf.Sin(angle * Mathf.Deg2Rad)));
            await UniTask.Delay(300);
        }
        SPDeactivate();
    }
}
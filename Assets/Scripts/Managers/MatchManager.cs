using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LitMotion;
using R3;
using UnityEngine;
using UnityEngine.UI;

public class MatchManager : MonoBehaviour
{
    public StageManager stage;
    public Character[] characters;
    public float timelimit = 180f;
    [SerializeField] TMPro.TMP_Text P1Hp;
    [SerializeField] TMPro.TMP_Text P2Hp;

    public float timer;

    void Start()
    {
        timer = timelimit;
        characters = FindObjectsByType<Character>(FindObjectsSortMode.None);

        foreach (var chara in characters)
        {
            chara.hp
                .Subscribe(async hp =>
                {
                    await HpTakeDamage(chara.PlayerID == 1 ? P1Hp : P2Hp);
                }).AddTo(this);

            chara.hp
                 .Select(hp => hp <= 0f)
                 .DistinctUntilChanged()
                 .Where(isDead => isDead)
                 .Subscribe(_ =>
                 {
                    Debug.Log(chara.characterName + " is dead!! ");
                    chara.hp.Value = 0f;
                    Destroy(chara.gameObject);
                 }).AddTo(this);
        }
    }

    void Update()
    {
        timer -= Time.deltaTime;

        foreach (var chara in characters)
        {
            if (chara == null) continue;
            if (stage.IsOutOfBounds(chara.transform))
            {
                Debug.Log(chara.characterName + " is dead!! ");
                chara.hp.Value = 0f;
            }
        }

        CheckVictory();
        DisplayHp();
    }

    void CheckVictory()
    {
        List<Character> alive = new List<Character>(System.Array.FindAll(characters, c => c.hp.Value > 0));

        if (alive.Count == 1)
        {
            Debug.Log("Winner is " + alive[0].characterName + "!!");
            enabled = false;
        }
        else if (alive.Count == 0)
        {
            Debug.Log("It's a draw!");
            enabled = false;
        }
        else if (timer <= 0f)
        {
            Character highestHpChar = null;
            float highestHp = -1f;

            foreach (var chara in alive)
            {
                if (chara.hp.Value > highestHp)
                {
                    highestHp = chara.hp.Value;
                    highestHpChar = chara;
                }
            }

            if (highestHpChar != null)
            {
                Debug.Log("Time's up! Winner is " + highestHpChar.characterName + "!!");
                enabled = false;
            }
            else
            {
                Debug.Log("Time's up! It's a draw!");
                enabled = false;
            }
        }
    }

    void DisplayHp()
    {
        if (characters.Length >= 2)
        {
            P1Hp.text = $"{characters[0].hp.Value} %";
            P2Hp.text = $"{characters[1].hp.Value} %";
        }
    }

    async UniTask HpTakeDamage(TMPro.TMP_Text hpText)
    {
        var oriSize = hpText.fontSize;
        await LMotion.Create(oriSize + 10f, oriSize, 0.2f)
                    .Bind(x =>
                    {
                        hpText.fontSize = x;
                    });
    }
}

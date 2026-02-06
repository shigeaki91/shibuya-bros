using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LitMotion;
using R3;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
using Extensions;

public class MatchManager : MonoBehaviour
{
    public StageManager stage;
    public Character[] characters = new Character[2];
    public float timelimit = 180f;
    [SerializeField] TMPro.TMP_Text P1Hp;
    [SerializeField] TMPro.TMP_Text P2Hp;
    [SerializeField] VideoPlayer _fatalEffectPlayer;
    RawImage _fatalEffectImage;
    [SerializeField] VideoPlayer _gameSetPlayer;
    RawImage _gameSetImage;
    [SerializeField] FadeController _fader;

    public float timer;

    void Awake()
    {
        _fatalEffectPlayer.Prepare();
        _gameSetPlayer.Prepare();
        _fatalEffectImage = _fatalEffectPlayer.GetComponent<RawImage>();
        _gameSetImage = _gameSetPlayer.GetComponent<RawImage>();
        _fatalEffectImage.enabled = false;
        _gameSetImage.enabled = false;        

        _fatalEffectPlayer.loopPointReached += OnVideoFinished;
        _gameSetPlayer.loopPointReached += OnVideoFinished;
    }
    void Start()
    {
        timer = timelimit;
        var charas = FindObjectsByType<Character>(FindObjectsSortMode.None);

        foreach (var chara in charas)
        {
            if (chara.PlayerID == 1) characters[0] = chara;
            else if (chara.PlayerID == 2) characters[1] = chara;
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
                    GameSet(chara).Forget();
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
            P1Hp.text = $"{characters[0].hp.Value:F1} %";
            P2Hp.text = $"{characters[1].hp.Value:F1} %";
        }
    }

    async UniTask HpTakeDamage(TMPro.TMP_Text hpText)
    {
        var oriSize = hpText.fontSize;
        await LMotion.Create(oriSize + 10f, oriSize, 0.2f)
                    .Bind(x =>
                    {
                        hpText.fontSize = x;
                    })
                    .ToUniTask();

    }

    async UniTask GameSet(Character loser)
    {
        AudioManager.Instance.StopBGM();
        loser.hp.Value = 0f;
        loser.Animator.SetBool("Dead", true);
        Time.timeScale = 0f;

        _fatalEffectImage.enabled = true;
        _fatalEffectPlayer.time = 0;
        _fatalEffectPlayer.Play();
        await _fatalEffectPlayer.WaitForEnd();

        Time.timeScale = 0.3f;
        await UniTask.Delay(1500, ignoreTimeScale: true);

        Time.timeScale = 0f;
        _gameSetImage.enabled = true;
        _gameSetPlayer.time = 0;
        _gameSetPlayer.Play();
        await _gameSetPlayer.WaitForEnd();

        Time.timeScale = 1f;
        await _fader.FadeIn();
        SceneManager.LoadScene("CharaSelect");
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        vp.gameObject.SetActive(false);
        vp.Stop();
    }
}

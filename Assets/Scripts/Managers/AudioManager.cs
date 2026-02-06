using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
struct BGMEntry
{
    public BGMtypes Type;
    public AudioClip Clip;
}
[System.Serializable]
struct SFXEntry
{
    public SFXtypes Type;
    public AudioClip Clip;
}

public class AudioManager : MonoBehaviour
{
    [SerializeField] BGMEntry[] _bgmEntry;
    [SerializeField] SFXEntry[] _sfxEntry;
    Dictionary<BGMtypes, AudioClip> _bgmDict;
    Dictionary<SFXtypes, AudioClip> _sfxDict;

    public static AudioManager Instance { get; private set; }

    private AudioSource _bgmSource;
    private AudioSource _sfxSource;

    void Awake()
    {
        if (Instance == null)
        {
            // RootLifetimeScopeで生成するため、DontDestroyOnLoadは不要

            Instance = this;

            _bgmSource = gameObject.AddComponent<AudioSource>();
            _sfxSource = gameObject.AddComponent<AudioSource>();
            
            _bgmDict = new Dictionary<BGMtypes, AudioClip>();
            foreach (var entry in _bgmEntry)
            {
                _bgmDict[entry.Type] = entry.Clip;
            }

            _sfxDict = new Dictionary<SFXtypes, AudioClip>();
            foreach (var entry in _sfxEntry)
            {
                _sfxDict[entry.Type] = entry.Clip;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayBGM(BGMtypes type, float volumeScale = 1.0f, bool loop = true)
    {
        if (_bgmDict.TryGetValue(type, out var clip))
        {
            _bgmSource.clip = clip;
            _bgmSource.loop = loop;
            _bgmSource.volume = volumeScale;
            _bgmSource.Play();
        }
        else
        {
            Debug.LogWarning($"BGM type {type} not found!");
        }
    }

    public void StopBGM()
    {
        _bgmSource.Stop();
    }

    public void PlaySFX(SFXtypes type, float volumeScale = 1.0f)
    {
        if (_sfxDict.TryGetValue(type, out var clip))
        {
            _sfxSource.PlayOneShot(clip, volumeScale);
        }
        else
        {
            Debug.LogWarning($"SFX type {type} not found!");
        }
    }

    public bool OnPlayingBGM()
    {
        return _bgmSource.isPlaying;
    }

    public bool OnPlayingSFX()
    {
        return _sfxSource.isPlaying;
    }

    public void StopAllSounds()
    {
        _bgmSource.Stop();
        _sfxSource.Stop();
    }
}

public enum BGMtypes
{
    MainTheme,
    BattleTheme,
}

public enum SFXtypes
{
    Jump,
    Attack,
    HitLight,
    HitHeavy,
    Jinari,
    Shakiinn,
    Select,
    GameStart,
    Dozun,
}
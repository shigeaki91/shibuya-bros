using System.Collections.Generic;
using UnityEngine;
using R3;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using LitMotion;
using System.Threading;

[System.Serializable]
struct CharaImageEntry
{
    public CharacterNames CharacterName;
    public Sprite CharacterImage;
}
public class SelectManager : MonoBehaviour
{
    public ReactiveProperty<int> _selectedIndex = new ReactiveProperty<int>(0);
    [SerializeField] UIShakeCameraStyle _cameraShake;
    [SerializeField] GameObject _charaSelectButtonPrefab;
    [SerializeField] List<CharaSelectButton> _charaSelectButtons;
    [SerializeField] CharaImageEntry[] _charaImageEntries;
    Dictionary<CharacterNames, Sprite> _charaImageDict;
    [SerializeField] List<Vector2> _buttonPositions;
    Image[] _selectedCharacterImages = new Image[2];
    
    [SerializeField] List<Vector2> _selectedCharacterImagePositions;
    [SerializeField] ReadyToFight _readyToFightButton;
    [SerializeField] GameObject _readyToFightButtonPrefab;

    [SerializeField] InputActionAsset _inputActionAsset;
    InputActionMap _inputActionMap;
    InputAction _backAction;
    SelectButton _backButton;
    [SerializeField] FadeController _faderToTitle;
    [SerializeField] FadeController _faderToMatch;
    CancellationTokenSource _ct = new CancellationTokenSource();
    [SerializeField] GameObject _buleLight;

    void Awake()
    {
        var i = 0;
        var charaNumber = System.Enum.GetValues(typeof(CharacterNames)).Length;
        _charaImageDict = new Dictionary<CharacterNames, Sprite>();
        foreach (var entry in _charaImageEntries)
        {
            _charaImageDict[entry.CharacterName] = entry.CharacterImage;

            var button = Instantiate(_charaSelectButtonPrefab, _cameraShake.transform).GetComponent<CharaSelectButton>();
            _charaSelectButtons.Add(button);
            button.CharacterName = entry.CharacterName;
            button.CharacterImage = entry.CharacterImage;
            button.transform.localPosition = _buttonPositions[i];
            i++;

            button.OnClicked.Subscribe(_ =>
            {
                Select(entry.CharacterName);
            }).AddTo(this);
        }

        _inputActionMap = _inputActionAsset.FindActionMap("CharaSelect");
        _backAction = _inputActionMap.FindAction("Back");
        _backAction.Enable();
        var inputObservable = Extensions.ObservableEx.InputActionAsObservable(_backAction);
        _backButton = new SelectButton(inputObservable);

        _backButton.OnPressed
            .Subscribe(_ => OnBackPressed())
            .AddTo(this);

        var chargeObservable = Extensions.ObservableEx.ChargeActionByObservable(_backAction, 1.0f);
        chargeObservable
            .Where(charge => charge >= 1.0f && _selectedIndex.Value == 0)
            .Subscribe(_ => 
            {
                GoBackToTitle().Forget();
                
            })
            
            .AddTo(this);

        _selectedIndex
            .Select(v => v == 2)
            .DistinctUntilChanged()
            .Subscribe(isReady =>
            {
                if (isReady)
                {
                    ReadyToFight(_ct.Token).Forget();
                }
                else
                {
                    _ct.Cancel();
                    if (_readyToFightButton != null)
                    {
                        Destroy(_readyToFightButton.gameObject);
                    }
                }
            })
            .AddTo(this);
    }

    async void Select(CharacterNames characterName)
    {
        Debug.Log($"{characterName} selected.");
        if (_selectedIndex.Value < 2)
        {
            GameManager.Instance.SetCharacters(characterName, _selectedIndex.Value);
            DisplaySelectedCharacter(_selectedIndex.Value, characterName);
            _selectedIndex.Value += 1;
            await _cameraShake.Shake(0.3f, 3f);
        }
    }

    void DisplaySelectedCharacter(int index, CharacterNames characterName)
    {
        var selectedCharacterImage = new GameObject("SelectedCharacterImage").AddComponent<Image>();
        selectedCharacterImage.transform.SetParent(_cameraShake.transform, false);
        selectedCharacterImage.transform.localPosition = _selectedCharacterImagePositions[index];
        selectedCharacterImage.sprite = _charaImageDict[characterName];
        _selectedCharacterImages[index] = selectedCharacterImage;
    }
    void OnBackPressed()
    {
        Debug.Log("Go Back One");
        if (_selectedIndex.Value > 0)
        {
            _selectedIndex.Value -= 1;
            Destroy(_selectedCharacterImages[_selectedIndex.Value].gameObject);
        }
    }
    async UniTask GoBackToTitle()
    {
        await _faderToTitle.FadeIn();
        SceneManager.LoadScene("Title");
    }

    async UniTask ReadyToFight(CancellationToken ct = default)
    {
        _readyToFightButton = Instantiate(_readyToFightButtonPrefab, _cameraShake.transform).GetComponent<ReadyToFight>();
        await LMotion.Create(1400f, 800f, 0.1f)
                        .Bind(x =>
                        {
                            _readyToFightButton.RectTransform.sizeDelta = new Vector2(x, _readyToFightButton.RectTransform.sizeDelta.y);
                        });

        _readyToFightButton.OnClicked
            .Subscribe(_ => 
            {
                _cameraShake.Shake(1f, 10f).Forget();
                BlueLightMove().Forget();
                MatchStart().Forget();
            }).AddTo(this);
    }

    async UniTask MatchStart(CancellationToken ct = default)
    {
        Debug.Log("Match Start");
        await _faderToMatch.FadeIn();
        Debug.Log("Load Match Scene");
        SceneManager.LoadScene("Match");
    }

    async UniTask BlueLightMove()
    {
        _buleLight.transform.SetAsLastSibling();
        var rect = _buleLight.GetComponent<RectTransform>();
        await LMotion.Create(-1000f, 0f, 1f)
            .Bind(t =>
            {
                rect.localPosition = new Vector3(t, rect.localPosition.y, rect.localPosition.z);
            }
            );
    }
}
using UnityEngine;
using UnityEngine.UI;
using LitMotion;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

public class BeforeMatchManager : MonoBehaviour
{
    [SerializeField] Image[] _selectedCharacterImages = new Image[2];
    [SerializeField] Image _vsImage;
    [SerializeField] FadeController _fader;
    void DisplayCharaImage(Sprite charaSprite, int index)
    {
        _selectedCharacterImages[index].enabled = true;
        _selectedCharacterImages[index].sprite = charaSprite;

        LMotion.Create(1000f, 350f, 0.5f)
            .WithEase(Ease.OutBounce)
            .Bind((v) =>
            {
                _selectedCharacterImages[index].rectTransform.sizeDelta = new Vector2(v, _selectedCharacterImages[index].rectTransform.sizeDelta.y);
            })
            .ToUniTask();
    }

    void DisplayVS()
    {
        _vsImage.enabled = true;

        LMotion.Create(800f, 300f, 0.5f)
            .WithEase(Ease.InExpo)
            .Bind((v) =>
            {
                _vsImage.rectTransform.sizeDelta = new Vector2(v, _vsImage.rectTransform.sizeDelta.y);
            })
            .ToUniTask();
    }

    async void Start()
    {
        _selectedCharacterImages[0].enabled = false;
        _selectedCharacterImages[1].enabled = false;
        _vsImage.enabled = false;
        await _fader.FadeOut();
        for (int i = 0; i < 2; i++)
        {
            var charaSprite = GameManager.Instance.selectedCharacterSprites[i];
            DisplayCharaImage(charaSprite, i);
            await Task.Delay(100);
        }

        await Task.Delay(500);

        DisplayVS();
        await Task.Delay(3000);

        SceneManager.LoadScene("Match");
    }
}
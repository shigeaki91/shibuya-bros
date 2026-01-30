using UnityEngine;
using UnityEngine.UI;
using R3;

public class ReadyToFight : MonoBehaviour
{
    public Button Button;
    public Observable<Unit> OnClicked { get; private set; }
    Image _image;
    public RectTransform RectTransform;

    void Awake()
    {
        Button = GetComponent<Button>();
        OnClicked = Button.OnClickAsObservable();
        _image = GetComponent<Image>();
        RectTransform = GetComponent<RectTransform>();
        _image.alphaHitTestMinimumThreshold = 0.5f;
    }
}
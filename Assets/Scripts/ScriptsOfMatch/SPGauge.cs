using UnityEngine;
using UnityEngine.UI;
using LitMotion;

public class SPGauge : MonoBehaviour
{
    public Character Owner;
    Image _gaugeImage;
    Image _frameImage;
    Image _OKImage;

    void Awake()
    {
        _gaugeImage = GetComponent<Image>();
        _frameImage = transform.Find("Frame").GetComponent<Image>();
        _OKImage = transform.Find("OK").GetComponent<Image>();
    }

    public void SetSPGauge(float spRatio)
    {
        _gaugeImage.fillAmount = spRatio;
    }

    void Update()
    {
        if (Owner == null) return;
        var ratio = Owner.GetSpecialChargeRatio();
        var fillAmount = _gaugeImage.fillAmount;        
        LMotion.Create(fillAmount, ratio, 0.2f)
            .WithEase(Ease.OutCubic)
            .Bind(t =>
            {
                _gaugeImage.fillAmount = t;
            });
        
        _OKImage.enabled = ratio >= 1.0f;
    }
}
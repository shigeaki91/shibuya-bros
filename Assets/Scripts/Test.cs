using UnityEngine;
using UnityEngine.UI;
using System;
using UniRx;
using TMPro;
using System.Xml.Linq;

public class Test : MonoBehaviour
{
    [SerializeField] private TimeCounter timeCounter;
    [SerializeField] private TMP_Text counterText; //uGUIのText

    Subject<string> subject = new Subject<string>();

    
}

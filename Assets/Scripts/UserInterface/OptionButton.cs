using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI bottomText;
    [SerializeField] private Image icon;

    public void Init(string text, Sprite sprite)
    {
        bottomText.text = text;
        icon.sprite = sprite;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PulseText : MonoBehaviour
{
    [SerializeField] private float toAlpha;
    [SerializeField] private float seconds;
    [SerializeField] private bool looping;

    private TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        Color newColor = text.color;
        Color fadeColor = newColor;
        fadeColor.a = toAlpha;

        if (looping)
        {
            LeanTween.value(gameObject, SetTextColor, fadeColor, newColor, seconds).setEase(LeanTweenType.easeOutQuad).setLoopPingPong();
        }
        else
        {
            LeanTween.value(gameObject, SetTextColor, fadeColor, newColor, seconds).setEase(LeanTweenType.easeOutQuad);
        }
    }

    private void SetTextColor(Color val)
    {
        text.color = val;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum ACCURACYINDICATOR { PERFECT, GREAT, GOOD, BAD, MISS }

public class AccuracyIndicatorController : MonoBehaviour
{
    private void Start()
    {
        LeanTween.move(GetComponent<RectTransform>(), new Vector2(-33f, 430f), 0.3f).setEase(LeanTweenType.easeInBounce).setOnComplete(FadeOut);
        LeanTween.alphaText(GetComponent<RectTransform>(), 1f, 0.15f);
    }

    private void FadeOut()
    {
        LeanTween.move(GetComponent<RectTransform>(), new Vector2(-33f, 400f), 0.1f).setEase(LeanTweenType.easeInCubic);
        LeanTween.alphaText(GetComponent<RectTransform>(), 0f, 0.1f).setDestroyOnComplete(true);
    }

    public void UpdateIndicator(ACCURACYINDICATOR accuracy)
    {
        TextMeshProUGUI indicatorText = GetComponent<TextMeshProUGUI>();

        switch (accuracy)
        {
            case ACCURACYINDICATOR.PERFECT:
                indicatorText.text = "Perfect!";
                break;
            case ACCURACYINDICATOR.GREAT:
                indicatorText.text = "Great!";
                break;
            case ACCURACYINDICATOR.GOOD:
                indicatorText.text = "Good!";
                break;
            case ACCURACYINDICATOR.BAD:
                indicatorText.text = "Bad!";
                break;
        }
    }
}

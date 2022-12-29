using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum ACCURACYINDICATOR { PERFECT, GREAT, GOOD, BAD, MISS }

public class AccuracyIndicatorController : MonoBehaviour
{
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

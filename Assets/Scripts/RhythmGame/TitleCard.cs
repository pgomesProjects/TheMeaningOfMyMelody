using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleCard : MonoBehaviour
{
    [SerializeField] private float slideInSeconds;
    [SerializeField] private float slideOutSeconds;
    [SerializeField] private float disappearDelay;

    // Start is called before the first frame update
    void Start()
    {
        LeanTween.move(GetComponent<RectTransform>(), new Vector2(0f, 310f), slideInSeconds).setEase(LeanTweenType.easeInOutExpo).setOnComplete(Disappear);
    }

    private void Disappear()
    {
        LeanTween.move(GetComponent<RectTransform>(), new Vector2(-450f, 310f), slideOutSeconds).setEase(LeanTweenType.easeInOutExpo).setDelay(disappearDelay);
    }
}

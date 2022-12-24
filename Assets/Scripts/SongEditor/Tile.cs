using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] private Color baseColor, offsetColor, hightlightColor;
    [SerializeField] private Image tileImage;

    private Color currentColor;

    public void CheckColor(bool isOffset)
    {
        tileImage.color = isOffset ? offsetColor : baseColor;
        currentColor = tileImage.color;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Click action here
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tileImage.color = hightlightColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tileImage.color = currentColor;
    }

}

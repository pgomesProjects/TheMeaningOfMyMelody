using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ControlButtonMouseEvents : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TextMeshProUGUI buttonText;
    private Color defaultTextColor;
    [SerializeField] private Color hoverTextColor = new Color(1, 1, 1, 1);
    [SerializeField] private Color selectedTextColor = new Color(1, 1, 1, 1);

    internal bool isHighlighted;

    private void Start()
    {
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        defaultTextColor = buttonText.color;
        isHighlighted = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        DialogController.main.isControlButtonHovered = true;

        //If the text is not stuck on being highlight (examples include the skip and auto functions)
        if (!isHighlighted)
        {
            //Change the text color when hovering over the text
            buttonText.color = hoverTextColor;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DialogController.main.isControlButtonHovered = false;
        //If the text is not stuck on being highlight (examples include the skip and auto functions)
        if (!isHighlighted)
        {
            //Change the text color back to normal when un-hovering the text
            buttonText.color = defaultTextColor;
        }
    }

    public void ToggleHighlight()
    {
        //Change the value of highlight to the opposite of what it currently is
        //Ex: if it's true, it'll turn false. If it's false, it'll turn true
        isHighlighted = !isHighlighted;

        //If the button is highlighted, keep the text stuck on the selected text color
        if(isHighlighted)
            buttonText.color = selectedTextColor;
        else
            buttonText.color = defaultTextColor;
    }
}

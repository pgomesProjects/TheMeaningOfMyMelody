using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class CutsceneEvent : DialogEvent
{
    [Header("Cutscene Objects")]
    [SerializeField][Tooltip("The container that holds all cutscene UI.")]
    protected GameObject cutsceneUI;
    [SerializeField]
    [Tooltip("The container that holds all dialog visuals.")]
    protected GameObject cutsceneVisuals;
    [SerializeField][Tooltip("The object that displays the sprite.")]
    protected Image dialogSprite;
    [SerializeField]
    [Tooltip("The object that displays the still images.")]
    protected GameObject stillSprite;
    [SerializeField][Tooltip("The object that displays the Name Box.")]
    protected GameObject nameBox;
    [SerializeField][Tooltip("The text box for the name box.")]
    protected TextMeshProUGUI nameText;
    [SerializeField]
    [Tooltip("A black screen used for cutscenes.")]
    protected GameObject blackScreen;

    [SerializeField][Tooltip("The images for the sprites in the cutscene.\nNote: The cutscene loads the first sprite given on start.")]
    protected Sprite[] spriteImages;

    [SerializeField]
    [Tooltip("The images for the stills in the cutscene.")]
    protected Sprite[] stillImages;

    //These are a bunch of different functions that cutscenes can call
    //They are here because they are functions that are likely to be used by multiple cutscenes
    public void ShowNameBox()
    {
        nameBox.SetActive(true);
    }

    public void SetNameBoxText(string name)
    {
        nameText.text = name;
    }

    public void HideNameBox()
    {
        nameBox.SetActive(false);
    }

    public void ChangeSprite(int index)
    {
        if(spriteImages.Length > 0)
        {
            dialogSprite.sprite = spriteImages[index];
        }
    }

    public void HideSprite()
    {
        dialogSprite.color = new Color(1, 1, 1, 0);
    }

    public void ShowSprite()
    {
        dialogSprite.color = new Color(1, 1, 1, 1);
    }

    public void HideStill()
    {
        stillSprite.GetComponent<CanvasGroup>().alpha = 0;
    }

    public void ShowStill()
    {
        stillSprite.GetComponent<CanvasGroup>().alpha = 1;
    }

    public IEnumerator FadeInStill(float seconds)
    {
        float currentTimer = 0;

        stillSprite.GetComponent<CanvasGroup>().alpha = 0;

        while (currentTimer < seconds)
        {
            currentTimer += Time.deltaTime;

            stillSprite.GetComponent<CanvasGroup>().alpha = Mathf.Clamp01(currentTimer / seconds);

            yield return null;
        }

        stillSprite.GetComponent<CanvasGroup>().alpha = 1;
    }

    public void ChangeStill(int index)
    {
        if(index >= 0 && index < stillImages.Length)
        {
            stillSprite.GetComponent<Image>().sprite = stillImages[index];
        }
    }

    public void SetVisualsOpacity(float alpha)
    {
        cutsceneVisuals.GetComponent<CanvasGroup>().alpha = alpha;
    }

    public void ShowBlackScreen()
    {
        blackScreen.SetActive(true);
    }

    public void HideBlackScreen()
    {
        blackScreen.SetActive(false);
    }

    public void NormalizeText()
    {
        messageText.fontStyle = TMPro.FontStyles.Normal;
    }

    public void ItalicizeText()
    {
        messageText.fontStyle = TMPro.FontStyles.Italic;
    }

    public void SetTextColor(Color newColor)
    {
        messageText.color = newColor;
    }

    public void ResetTextColor()
    {
        messageText.color = Color.black;
    }

    public void SpriteJump()
    {
        CutsceneController.main.spriteAnimator.SetTrigger("Jump");
    }
}

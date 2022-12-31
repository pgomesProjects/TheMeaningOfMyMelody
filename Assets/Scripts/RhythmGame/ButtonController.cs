using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NOTETYPE { LEFT, DOWN, UP, RIGHT }

public class ButtonController : MonoBehaviour
{
    [SerializeField] internal NOTETYPE buttonType;
    [SerializeField] protected CharacterAnimationController characterAnimator;

    private Color defaultColor;
    [SerializeField] protected Color pressColor;

    protected SpriteRenderer arrowSpriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        arrowSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        defaultColor = arrowSpriteRenderer.color;
    }

    protected void ButtonPressAction()
    {
        arrowSpriteRenderer.color = pressColor;
        StartCharacterDirection();
    }

    protected void ButtonLiftAction()
    {
        arrowSpriteRenderer.color = defaultColor;
        arrowSpriteRenderer.gameObject.transform.localScale = Vector3.one;
        StopCharacterDirection();
    }

    private void StartCharacterDirection()
    {
        if(characterAnimator != null)
        {
            switch (buttonType)
            {
                case NOTETYPE.LEFT:
                    characterAnimator.CharacterDirection("Left");
                    break;
                case NOTETYPE.DOWN:
                    characterAnimator.CharacterDirection("Down");
                    break;
                case NOTETYPE.UP:
                    characterAnimator.CharacterDirection("Up");
                    break;
                case NOTETYPE.RIGHT:
                    characterAnimator.CharacterDirection("Right");
                    break;
            }
        }
    }

    private void StopCharacterDirection()
    {
        if(characterAnimator != null)
        {
            switch (buttonType)
            {
                case NOTETYPE.LEFT:
                    characterAnimator.ResetCharacterDirection("Left");
                    break;
                case NOTETYPE.DOWN:
                    characterAnimator.ResetCharacterDirection("Down");
                    break;
                case NOTETYPE.UP:
                    characterAnimator.ResetCharacterDirection("Up");
                    break;
                case NOTETYPE.RIGHT:
                    characterAnimator.ResetCharacterDirection("Right");
                    break;
            }
        }
    }
}

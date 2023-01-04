using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerButtonController : ButtonController
{
    private PlayerControls playerControls;
    private float pressedScale = 1.05f;

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        #region BUTTONENABLECONTROLS
        switch (buttonType)
        {
            case NOTETYPE.LEFT:
                playerControls.Player.LeftArrow.Enable();
                playerControls.Player.LeftArrow.performed += OnButtonPress;
                playerControls.Player.LeftArrow.canceled += OnButtonLift;
                break;
            case NOTETYPE.DOWN:
                playerControls.Player.DownArrow.Enable();
                playerControls.Player.DownArrow.performed += OnButtonPress;
                playerControls.Player.DownArrow.canceled += OnButtonLift;
                break;
            case NOTETYPE.UP:
                playerControls.Player.UpArrow.Enable();
                playerControls.Player.UpArrow.performed += OnButtonPress;
                playerControls.Player.UpArrow.canceled += OnButtonLift;
                break;
            case NOTETYPE.RIGHT:
                playerControls.Player.RightArrow.Enable();
                playerControls.Player.RightArrow.performed += OnButtonPress;
                playerControls.Player.RightArrow.canceled += OnButtonLift;
                break;
        }
        #endregion
    }

    private void OnDisable()
    {
        #region BUTTONDISABLECONTROLS
        switch (buttonType)
        {
            case NOTETYPE.LEFT:
                playerControls.Player.LeftArrow.Disable();
                playerControls.Player.LeftArrow.performed -= OnButtonPress;
                playerControls.Player.LeftArrow.canceled -= OnButtonLift;
                break;
            case NOTETYPE.DOWN:
                playerControls.Player.DownArrow.Disable();
                playerControls.Player.DownArrow.performed -= OnButtonPress;
                playerControls.Player.DownArrow.canceled -= OnButtonLift;
                break;
            case NOTETYPE.UP:
                playerControls.Player.UpArrow.Disable();
                playerControls.Player.UpArrow.performed -= OnButtonPress;
                playerControls.Player.UpArrow.canceled -= OnButtonLift;
                break;
            case NOTETYPE.RIGHT:
                playerControls.Player.RightArrow.Disable();
                playerControls.Player.RightArrow.performed -= OnButtonPress;
                playerControls.Player.RightArrow.canceled -= OnButtonLift;
                break;
        }
        #endregion
    }

    private void OnButtonPress(InputAction.CallbackContext ctx)
    {
        if (LevelManager.IsSongPlaying())
        {
            arrowSpriteRenderer.gameObject.transform.localScale = new Vector3(pressedScale, pressedScale, pressedScale);
            ButtonPressAction();
        }
    }

    private void OnButtonLift(InputAction.CallbackContext ctx)
    {
        if (LevelManager.IsSongPlaying())
        {
            arrowSpriteRenderer.gameObject.transform.localScale = Vector3.one;
            ButtonLiftAction();
        }
    }
}

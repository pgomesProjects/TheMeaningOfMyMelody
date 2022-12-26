using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum NOTETYPE { LEFT, DOWN, UP, RIGHT }

public class ButtonController : MonoBehaviour
{
    private PlayerControls playerControls;

    [SerializeField] private NOTETYPE buttonType;

    private Color defaultColor;
    [SerializeField] private Color pressColor;
    private float pressedScale = 1.05f;

    private SpriteRenderer arrowSpriteRenderer;

    private bool isPressed;

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    // Start is called before the first frame update
    void Start()
    {
        arrowSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        defaultColor = arrowSpriteRenderer.color;
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

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Note")
        {
            if(isPressed)
                collision.GetComponent<NoteController>().NoteHit();
        }
    }

    private void OnButtonPress(InputAction.CallbackContext ctx)
    {
        arrowSpriteRenderer.color = pressColor;
        isPressed = true;
        arrowSpriteRenderer.gameObject.transform.localScale = new Vector3(pressedScale, pressedScale, pressedScale);
    }

    private void OnButtonLift(InputAction.CallbackContext ctx)
    {
        arrowSpriteRenderer.color = defaultColor;
        isPressed = false;
        arrowSpriteRenderer.gameObject.transform.localScale = Vector3.one;
    }

    public bool IsPressed() => isPressed;
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject songEditor;
    private PlayerControls playerControls;

    private void Awake()
    {
        playerControls = new PlayerControls();
        songEditor.SetActive(false);
    }

    private void OnEnable()
    {
        playerControls.Player.OpenSongEditor.Enable();
        playerControls.Player.OpenSongEditor.performed += OpenSongEditor;
        playerControls.Player.CloseSongEditor.Enable();
        playerControls.Player.CloseSongEditor.performed += CloseSongEditor;
    }

    private void OnDisable()
    {
        playerControls.Player.OpenSongEditor.Disable();
        playerControls.Player.OpenSongEditor.performed -= OpenSongEditor;
        playerControls.Player.CloseSongEditor.Disable();
        playerControls.Player.CloseSongEditor.performed -= CloseSongEditor;
    }

    public void OpenSongEditor(InputAction.CallbackContext ctx)
    {
        Debug.Log("Opening Song Editor...");
        songEditor.SetActive(true);
    }

    public void CloseSongEditor(InputAction.CallbackContext ctx)
    {
        Debug.Log("Closing Song Editor...");
        songEditor.SetActive(false);
    }
}

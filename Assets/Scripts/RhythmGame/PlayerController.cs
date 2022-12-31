using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject songEditor;
    private SongEditorManager _songEditorManager;
    [SerializeField] private InputAction changeBeatSnap;
    private PlayerControls playerControls;
    private bool inSongEditor;

    private void Awake()
    {
        playerControls = new PlayerControls();
        songEditor.SetActive(false);
        _songEditorManager = songEditor.GetComponent<SongEditorManager>();
        inSongEditor = false;
    }

    private void OnEnable()
    {
        playerControls.Player.OpenSongEditor.Enable();
        playerControls.Player.OpenSongEditor.performed += OpenSongEditor;
        playerControls.Player.CloseSongEditor.Enable();
        playerControls.Player.CloseSongEditor.performed += CloseSongEditor;
        changeBeatSnap.Enable();
        changeBeatSnap.performed += ChangeBeatSnap;
    }

    private void OnDisable()
    {
        playerControls.Player.OpenSongEditor.Disable();
        playerControls.Player.OpenSongEditor.performed -= OpenSongEditor;
        playerControls.Player.CloseSongEditor.Disable();
        playerControls.Player.CloseSongEditor.performed -= CloseSongEditor;
        changeBeatSnap.Disable();
        changeBeatSnap.performed -= ChangeBeatSnap;
    }

    public void OpenSongEditor(InputAction.CallbackContext ctx)
    {
        Debug.Log("Opening Song Editor...");
        songEditor.SetActive(true);
        inSongEditor = true;
    }

    public void CloseSongEditor(InputAction.CallbackContext ctx)
    {
        Debug.Log("Closing Song Editor...");
        songEditor.SetActive(false);
        inSongEditor = false;
    }

    public void ChangeBeatSnap(InputAction.CallbackContext ctx)
    {
        if (inSongEditor)
        {
            float inputValue = ctx.ReadValue<float>();
            switch (inputValue)
            {
                case -1f:
                    _songEditorManager.DecreaseBeatSnap();
                    break;
                case 1f:
                    _songEditorManager.IncreaseBeatSnap();
                    break;
            }
        }
    }
}

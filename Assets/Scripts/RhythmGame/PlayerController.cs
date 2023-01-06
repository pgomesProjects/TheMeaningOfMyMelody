using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject songEditor;
    private SongEditorManager _songEditorManager;
    [SerializeField] private InputAction changeBeatSnap;
    [SerializeField] private InputAction changeSection;
    [SerializeField] private InputAction changeStrumSnap;
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
        changeSection.Enable();
        changeSection.performed += ChangeSection;
        changeStrumSnap.Enable();
        changeStrumSnap.performed += ChangeStepSnap;
    }

    private void OnDisable()
    {
        playerControls.Player.OpenSongEditor.Disable();
        playerControls.Player.OpenSongEditor.performed -= OpenSongEditor;
        playerControls.Player.CloseSongEditor.Disable();
        playerControls.Player.CloseSongEditor.performed -= CloseSongEditor;
        changeBeatSnap.Disable();
        changeBeatSnap.performed -= ChangeBeatSnap;
        changeSection.Disable();
        changeSection.performed -= ChangeSection;
        changeStrumSnap.Disable();
        changeStrumSnap.performed -= ChangeStepSnap;
    }

    public void OpenSongEditor(InputAction.CallbackContext ctx)
    {
        Debug.Log("Opening Song Editor...");
        songEditor.SetActive(true);
        inSongEditor = true;
        FindObjectOfType<AudioManager>().PauseAllSounds();
    }

    public void CloseSongEditor(InputAction.CallbackContext ctx)
    {
        Debug.Log("Closing Song Editor...");
        inSongEditor = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        songEditor.SetActive(false);
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

    public void ChangeSection(InputAction.CallbackContext ctx)
    {
        if (inSongEditor)
        {
            float inputValue = ctx.ReadValue<float>();
            switch (inputValue)
            {
                case -1f:
                    _songEditorManager.ChangeSection((int)inputValue);
                    break;
                case 1f:
                    _songEditorManager.ChangeSection((int)inputValue);
                    break;
            }
        }
    }

    public void ChangeStepSnap(InputAction.CallbackContext ctx)
    {
        if (inSongEditor)
        {
            float inputValue = ctx.ReadValue<float>();
            switch (inputValue)
            {
                case -1f:
                    _songEditorManager.SnapStrumPosition((int)inputValue);
                    break;
                case 1f:
                    _songEditorManager.SnapStrumPosition((int)inputValue);
                    break;
            }
        }
    }
}

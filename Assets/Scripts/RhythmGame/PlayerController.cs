using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject songEditor;
    private SongEditorManager _songEditorManager;
    [SerializeField] private InputAction scrollChart;
    [SerializeField] private InputAction changeBeatSnap;
    [SerializeField] private InputAction changeSection;
    [SerializeField] private InputAction changeStrumSnap;
    private PlayerControls playerControls;

    private void Awake()
    {
        playerControls = new PlayerControls();
        songEditor.SetActive(false);
        _songEditorManager = songEditor.GetComponent<SongEditorManager>();
    }

    private void OnEnable()
    {
        playerControls.Player.OpenSongEditor.Enable();
        playerControls.Player.OpenSongEditor.performed += OpenSongEditor;
        playerControls.Player.CloseSongEditor.Enable();
        playerControls.Player.CloseSongEditor.performed += CloseSongEditor;
        scrollChart.Enable();
        scrollChart.performed += OnChartScroll;
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
        scrollChart.Disable();
        scrollChart.performed -= OnChartScroll;
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
        LevelManager.Instance.SetInSongEditor(true);
        FindObjectOfType<AudioManager>().PauseAllSounds();
    }

    public void CloseSongEditor(InputAction.CallbackContext ctx)
    {
        Debug.Log("Closing Song Editor...");
        FindObjectOfType<AudioManager>().ResumeAllSounds();
        LevelManager.Instance.SetInSongEditor(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        songEditor.SetActive(false);
    }

    public void OnChartScroll(InputAction.CallbackContext ctx)
    {
        if (LevelManager.Instance.InSongEditor())
        {
            float inputValue = ctx.ReadValue<float>();
            Debug.Log("Scroll Value: " + inputValue);
            if(inputValue < 0)
            {
                _songEditorManager.ChangeStrumPosition(-(_songEditorManager.GetBeatChartValue() / 5f));
            }
            else if(inputValue > 0)
            {
                _songEditorManager.ChangeStrumPosition(_songEditorManager.GetBeatChartValue() / 5f);
            }
        }
    }

    public void ChangeBeatSnap(InputAction.CallbackContext ctx)
    {
        if (LevelManager.Instance.InSongEditor())
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
        if (LevelManager.Instance.InSongEditor())
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
        if (LevelManager.Instance.InSongEditor())
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

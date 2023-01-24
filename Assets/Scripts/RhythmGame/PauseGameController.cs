using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseGameController : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    private AudioManager audioManager;

    private PlayerControls playerControls;

    private void Awake()
    {
        playerControls = new PlayerControls();
        playerControls.UI.Pause.performed += _ => TogglePause();
    }

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();   
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void TogglePause()
    {
        //Toggle pause
        LevelManager.Instance.SetPaused(!LevelManager.Instance.IsPaused());

        //Paused
        if (LevelManager.Instance.IsPaused())
        {
            Debug.Log("Paused");
            pauseMenu.SetActive(true);
            if (audioManager != null)
                audioManager.PauseAllSounds();
        }

        //Unpaused
        else
        {
            Debug.Log("Unpaused");
            pauseMenu.SetActive(false);
            if (audioManager != null)
                audioManager.ResumeAllSounds();
        }
    }

    public void BackToMain()
    {
        if (audioManager != null)
            audioManager.StopAllSounds();
        SceneManager.LoadScene("Menu");
    }

    public void Unpause()
    {
        LevelManager.Instance.SetPaused(false);
        pauseMenu.SetActive(false);
        if (audioManager != null)
            audioManager.ResumeAllSounds();
    }
}

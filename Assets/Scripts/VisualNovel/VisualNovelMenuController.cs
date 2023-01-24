using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VisualNovelMenuController : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    
    private PlayerControls playerControls;

    private void Awake()
    {
        playerControls = new PlayerControls();
        playerControls.UI.Pause.performed += _ => TogglePause();
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
        CutsceneController.main.SetPaused(!CutsceneController.main.IsPaused());

        //Paused
        if (CutsceneController.main.IsPaused())
        {
            Debug.Log("Paused");
            menu.SetActive(true);
        }

        //Unpaused
        else
        {
            Debug.Log("Unpaused");
            menu.SetActive(false);
        }
    }

    public void BackToMain()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Unpause()
    {
        CutsceneController.main.SetPaused(false);
        menu.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MenuState { START, MAIN, STORY, FREEPLAY, CONFIG, CREDITS }

public class MainMenuController : MonoBehaviour
{
    private GameObject currentMenuState;

    [SerializeField] private GameObject startMenu;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject storyMenu;
    [SerializeField] private GameObject freeplayMenu;
    [SerializeField] private GameObject configMenu;
    [SerializeField] private GameObject creditsMenu;

    private PlayerControls playerControls;

    private void Awake()
    {
        playerControls = new PlayerControls();
        playerControls.UI.AnyButton.performed += _ => StartToMain();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentMenuState = startMenu;

        if (GameData.gameEntered)
            SwitchMenu(GameData.returnOnMenuState);
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void StartToMain()
    {
        SwitchMenu(MenuState.MAIN);
        GameData.gameEntered = true;
    }

    public void StoryMode()
    {
        SwitchMenu(MenuState.STORY);
    }

    public void Back()
    {
        SwitchMenu(MenuState.MAIN);
    }

    private void SwitchMenu(MenuState menu)
    {
        GameObject newMenu;

        switch (menu)
        {
            case MenuState.START:
                newMenu = startMenu;
                break;
            case MenuState.MAIN:
                newMenu = mainMenu;
                break;
            case MenuState.STORY:
                newMenu = storyMenu;
                break;
            case MenuState.FREEPLAY:
                newMenu = freeplayMenu;
                break;
            case MenuState.CONFIG:
                newMenu = configMenu;
                break;
            case MenuState.CREDITS:
                newMenu = creditsMenu;
                break;
            default:
                newMenu = mainMenu;
                break;
        }

        currentMenuState.SetActive(false);

        currentMenuState = newMenu;
        currentMenuState.SetActive(true);
    }

        public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}

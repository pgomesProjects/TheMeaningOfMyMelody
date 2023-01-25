using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugModeHandler : MonoBehaviour
{
    [SerializeField] private GameObject debugMenu;

    private void Start()
    {
        UpdateDebugUI();
    }

    public void UpdateDebugUI()
    {
        if (GameSettings.debugMode)
            debugMenu.SetActive(true);
        else
            debugMenu.SetActive(false);
    }
}

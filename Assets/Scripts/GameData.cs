using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameData
{
    internal static bool gameEntered = false;
    internal static TextAsset currentScriptAsset;
    internal static TextAsset currentScriptEventsAsset;
    internal static List<string> historyLog = new List<string>();

    internal static MenuState returnOnMenuState = MenuState.START;
}
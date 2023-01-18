using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
abstract public class DialogEvent : MonoBehaviour
{
    protected int currentLine;

    //The basic objects that any dialog event will need
    [Header("Dialog Objects")]
    protected string[] dialogLines;
    protected bool dialogWrittenInHistory;
    [SerializeField] [Tooltip("The object that holds the message text.")]
    protected TextMeshProUGUI messageText;
    [SerializeField] [Tooltip("The object that holds whatever will be used to tell the player to continue the dialog.")]
    protected GameObject continueObject;

    //Template for checking for events in the dialog
    public abstract void CheckEvents(ref TextWriter.TextWriterSingle textWriterObj);

    public int GetCurrentLine() { return this.currentLine; }
    public int GetDialogLength() { return this.dialogLines.Length; }
    public abstract void OnDialogStart();
    public abstract void OnEventComplete();

    public void GenerateDialogLines()
    {
        string[] rawTextList = GameData.currentScriptAsset.text.Split('\n');
        List<string> allLines = new List<string>();
        foreach(var line in rawTextList)
        {
            string[] currentRawLine = line.Split(',');
            if(currentRawLine.Length > 1)
            {
                allLines.Add(currentRawLine[1]);
            }
            else
            {
                allLines.Add(currentRawLine[0]);
            }
        }

       dialogLines = allLines.ToArray();
    }
}

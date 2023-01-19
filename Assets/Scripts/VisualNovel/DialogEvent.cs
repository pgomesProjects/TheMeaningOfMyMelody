using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
abstract public class DialogEvent : MonoBehaviour
{
    public struct DialogLines
    {
        public string name;
        public string line;
    }

    protected int currentLine;

    //The basic objects that any dialog event will need
    [Header("Dialog Objects")]
    protected DialogLines[] dialogLines;
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
        List<DialogLines> allLines = new List<DialogLines>();
        foreach(var line in rawTextList)
        {
            string[] currentRawLine = line.Split(',');
            DialogLines currentDialogLine = new DialogLines();
            if(currentRawLine.Length > 1)
            {
                currentDialogLine.name = currentRawLine[0];
                currentDialogLine.line = currentRawLine[1];
            }
            else
            {
                currentDialogLine.line = currentRawLine[0];
            }

            allLines.Add(currentDialogLine);
        }

       dialogLines = allLines.ToArray();

        
    }
}

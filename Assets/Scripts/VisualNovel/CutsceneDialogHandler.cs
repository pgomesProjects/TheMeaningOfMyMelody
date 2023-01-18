using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneDialogHandler : CutsceneEvent
{
    private CustomEvent cutsceneCustomEvents;
    internal bool canAutoAdvance = false;
    internal bool forceSkip = false;
    internal bool textCompleted = false;
    [SerializeField] private float skipTextMultiplier = 1.5f;
    private float timeToReadText;

    private bool hideCommandActive;

    private void Awake()
    {
        cutsceneCustomEvents = GetComponent<CustomEvent>();
    }

    public override void OnDialogStart()
    {
        //Make sure the text speed is normal and then show the cutscene UI
        CutsceneController.main.currentTextSpeed = CutsceneController.main.textSpeed;
        cutsceneUI.SetActive(true);

        //These are the default options for the cutscene
        ChangeSprite(0);
        SetNameBoxText("Character Name");
    }

    public override void CheckEvents(ref TextWriter.TextWriterSingle textWriterObj)
    {
        string message = dialogLines[currentLine];

        //Get the text to read the text (based on text count and text speed)
        timeToReadText = message.Length / CutsceneController.main.currentTextSpeed;

        //If the lines have not been written into the history log, write them in
        if (!dialogWrittenInHistory && !CheckForCommands(message, ref textWriterObj))
        {
            //Add to history log
            if (nameBox.activeInHierarchy)
                DialogController.main.AddToLog(nameText.text + ":<br>");
            DialogController.main.AddToLog(message + "<br><br>");
        }

        textCompleted = false;

        //Check for custom events if present
        if (cutsceneCustomEvents != null)
            cutsceneCustomEvents.CheckForCustomEvent(currentLine);

        //Hide the continue object while the text is being displayed
        continueObject.SetActive(false);

        //If the text is skipping, multiply the speed by the defined skip speed multiplier
        if (CutsceneController.main.isSkipping)
            CutsceneController.main.currentTextSpeed = CutsceneController.main.textSpeed * CutsceneController.main.skipSpeedMultiplier;

        Debug.Log("Current Text Speed: " + CutsceneController.main.currentTextSpeed);

        //Give the player at least a half second to read text
        if (timeToReadText < 0.5f)
        {
            timeToReadText = 0.5f;
        }

        //Use the text writer class to write each character one by one
        textWriterObj = TextWriter.AddWriter_Static(null, messageText, message, 1 / CutsceneController.main.currentTextSpeed, true, true, OnTextComplete);
        //Move to the next line in the dialog
        currentLine++;
    }

    private bool CheckForCommands(string command, ref TextWriter.TextWriterSingle textWriterObj)
    {
        //Resets commands that are only active for one line of dialog
        ResetOneShotCommands();

        //If the current line has valid command context
        if (command[0] == '[' && command[command.Length - 1] == ']')
        {
            //Remove the brackets from the command
            command = command.Remove(0, 1);
            command = command.Remove(command.Length - 1, 1);

            //List of dialog commands
            switch (command)
            {
                case "HIDE":
                    Debug.Log("HIDE COMMAND CALLED!");
                    HideCommand();

                    //If the cutscene is on auto mode, let the image display for 2 seconds
                    if(CutsceneController.main.isAuto)
                        timeToReadText = 2;
                    //If not, let the player exit whenever they want by clicking
                    else
                        textWriterObj.WriteAllAndDestroy();
                    return true;
            }
        }

        return false;
    }

    private void ResetOneShotCommands()
    {
        if (hideCommandActive)
        {
            SetVisualsOpacity(1);
            hideCommandActive = false;
        }
    }

    private void HideCommand()
    {
        SetVisualsOpacity(0);
        hideCommandActive = true;
    }

    public void SetForceSkip(bool skip)
    {
        forceSkip = skip;
        if (!CutsceneController.main.isSkipping)
        {
            CutsceneController.main.forceSkip = forceSkip;
            if (forceSkip)
            {
                CutsceneController.main.currentTextSpeed = CutsceneController.main.textSpeed * skipTextMultiplier;
            }
            else
                CutsceneController.main.currentTextSpeed = CutsceneController.main.textSpeed;
        }
    }

    public void CheckForceSkip()
    {
        if (forceSkip)
            CutsceneController.main.currentTextSpeed = CutsceneController.main.textSpeed * skipTextMultiplier;
        else
            CutsceneController.main.currentTextSpeed = CutsceneController.main.textSpeed;
    }

    public void OnTextComplete()
    {
        textCompleted = true;

        //If the text is being skipped, call the skip text function to forcefully advance the text
        if (forceSkip || CutsceneController.main.isSkipping)
        {
            SkipText();
        }
        //If the text is being auto read, call the function that makes it have a delay before the text auto-advances
        else if (CutsceneController.main.isAuto)
        {
            AutoAdvanceText();
        }
        //If neither, show the continue object so the player knows to advance the text themselves
        else
        {
            continueObject.SetActive(true);
        }
    }

    private void SkipText()
    {
        StartCoroutine(CutsceneController.main.ForceAdvance());
    }

    private void AutoAdvanceText()
    {
        Debug.Log("Time To Read Text: " + timeToReadText + " seconds");
        StartCoroutine(CutsceneController.main.AutoAdvance(timeToReadText));
    }

    public override void OnEventComplete()
    {
        //Reset values
        ControlButtonMouseEvents[] allButtons = FindObjectsOfType<ControlButtonMouseEvents>();

        //Turn off all buttons that are currently highlighted or triggered
        foreach(var i in allButtons)
        {
            if (i.isHighlighted)
                i.ToggleHighlight();
        }

        //If skipping or auto, toggle them off
        if (CutsceneController.main.isSkipping)
            DialogController.main.ToggleSkip();

        if (CutsceneController.main.isAuto)
            DialogController.main.ToggleAuto();

        //Hide the dialog box and continue object
        cutsceneUI.SetActive(false);
        continueObject.SetActive(false);

        //Reset lines
        currentLine = 0;

        //Check for custom events if present
        if (cutsceneCustomEvents != null)
            cutsceneCustomEvents.CustomOnEventComplete();
    }
}

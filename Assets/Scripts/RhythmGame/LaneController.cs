using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class LaneController : MonoBehaviour
{
    private PlayerControls playerControls;

    [SerializeField] private CHARTTYPE chartType;
    [SerializeField] private NOTETYPE noteDirection;
    [SerializeField] private GameObject notePrefab;
    private List<NoteController> notes;
    private List<double> timeStamps;
    private int spawnIndex, inputIndex;
    private bool notePressed = false;

    private TransparencyController transparencyController;
    private List<OpponentButtonController> opponentButtons;

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    // Start is called before the first frame update
    void Start()
    {
        notes = new List<NoteController>();
        timeStamps = new List<double>();
        if (chartType == CHARTTYPE.OPPONENT)
        {
            opponentButtons = new List<OpponentButtonController>();
            opponentButtons.AddRange(FindObjectsOfType<OpponentButtonController>());
            opponentButtons = opponentButtons.OrderBy(button => button.buttonType).ToList();
        }

        transparencyController = transform.parent.parent.GetComponent<TransparencyController>();
    }

    private void OnEnable()
    {
        #region BUTTONENABLECONTROLS
        switch (noteDirection)
        {
            case NOTETYPE.LEFT:
                playerControls.Player.LeftArrow.Enable();
                playerControls.Player.LeftArrow.started += OnButtonPress;
                playerControls.Player.LeftArrow.canceled += OnButtonLift;
                break;
            case NOTETYPE.DOWN:
                playerControls.Player.DownArrow.Enable();
                playerControls.Player.DownArrow.started += OnButtonPress;
                playerControls.Player.DownArrow.canceled += OnButtonLift;
                break;
            case NOTETYPE.UP:
                playerControls.Player.UpArrow.Enable();
                playerControls.Player.UpArrow.started += OnButtonPress;
                playerControls.Player.UpArrow.canceled += OnButtonLift;
                break;
            case NOTETYPE.RIGHT:
                playerControls.Player.RightArrow.Enable();
                playerControls.Player.RightArrow.started += OnButtonPress;
                playerControls.Player.RightArrow.canceled += OnButtonLift;
                break;
        }
        #endregion
    }

    private void OnDisable()
    {
        #region BUTTONDISABLECONTROLS
        switch (noteDirection)
        {
            case NOTETYPE.LEFT:
                playerControls.Player.LeftArrow.Disable();
                playerControls.Player.LeftArrow.started -= OnButtonPress;
                playerControls.Player.LeftArrow.canceled -= OnButtonLift;
                break;
            case NOTETYPE.DOWN:
                playerControls.Player.DownArrow.Disable();
                playerControls.Player.DownArrow.started -= OnButtonPress;
                playerControls.Player.DownArrow.canceled += OnButtonLift;
                break;
            case NOTETYPE.UP:
                playerControls.Player.UpArrow.Disable();
                playerControls.Player.UpArrow.started -= OnButtonPress;
                playerControls.Player.UpArrow.canceled += OnButtonLift;
                break;
            case NOTETYPE.RIGHT:
                playerControls.Player.RightArrow.Disable();
                playerControls.Player.RightArrow.started -= OnButtonPress;
                playerControls.Player.RightArrow.canceled += OnButtonLift;
                break;
        }
        #endregion
    }

    public void AddNote(double timeStamp)
    {
        timeStamps.Add(timeStamp);
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelManager.IsSongPlaying())
        {
            if (spawnIndex < timeStamps.Count)
            {
                if (LevelManager.GetSongTime() >= timeStamps[spawnIndex] - LevelManager.Instance.GetScrollSpeedTime())
                {
                    //Debug.Log("Note Spawned At: " + LevelManager.GetSongTime());
                    var note = Instantiate(notePrefab, transform);
                    //If the grandparent has a transparency controller, control the transparency of the notes
                    if (transparencyController != null)
                    {
                        Color adjustAlpha = note.GetComponentInChildren<SpriteRenderer>().color;
                        adjustAlpha.a = transparencyController.GetAlpha();
                        note.GetComponentInChildren<SpriteRenderer>().color = adjustAlpha;
                        //Debug.Log("Alpha Adjusted: " + adjustAlpha.a);
                    }
                    NoteController currentNote = note.GetComponent<NoteController>();
                    currentNote.SetNoteDirection(noteDirection);
                    currentNote.SetAssignedTime((float)timeStamps[spawnIndex]);
                    notes.Add(currentNote);
                    spawnIndex++;
                }
            }

            if (inputIndex < timeStamps.Count)
            {
                double timeStamp = timeStamps[inputIndex];
                double marginOfError = LevelManager.Instance.marginOfError;
                double audioTime = LevelManager.GetSongTime() - (LevelManager.Instance.inputDelayMilliseconds / 1000.0);

                switch (chartType)
                {
                    case CHARTTYPE.OPPONENT:
                        if (audioTime >= timeStamp)
                        {
                            opponentButtons[(int)noteDirection].HitButton();
                            Destroy(notes[inputIndex].gameObject);
                            inputIndex++;
                        }
                        break;
                    case CHARTTYPE.PLAYER:
                        if (notePressed)
                        {
                            double hitDelay = Math.Abs(audioTime - timeStamp);

                            if (hitDelay < marginOfError)
                            {
                                Hit(hitDelay);
                                Destroy(notes[inputIndex].gameObject);
                                inputIndex++;
                            }
                            else
                            {
                                //print($"Hit inaccurate on {inputIndex} note with {Math.Abs(audioTime - timeStamp)} delay");
                            }
                        }
                        if (timeStamp + marginOfError <= audioTime)
                        {
                            Miss();
                            inputIndex++;
                        }
                        break;
                }
            }
        }
    }

    private void OnButtonPress(InputAction.CallbackContext ctx)
    {
        if (ctx.action.WasPressedThisFrame())
        {
            notePressed = true;
        }
        else
        {
            notePressed = false;
        }
    }

    private void OnButtonLift(InputAction.CallbackContext ctx)
    {
        notePressed = false;
    }

    private void Hit(double hitDelay)
    {
        //print($"Hit on {inputIndex} note");

        //Perfect hit
        if (hitDelay <= LevelManager.Instance.perfectHitMargin)
        {
            LevelManager.Instance.ShowAccuracyIndicator(ACCURACYINDICATOR.PERFECT);
        }
        //Great hit
        else if (hitDelay <= LevelManager.Instance.greatHitMargin)
        {
            LevelManager.Instance.ShowAccuracyIndicator(ACCURACYINDICATOR.GREAT);
        }
        //Good hit
        else if (hitDelay <= LevelManager.Instance.goodHitMargin)
        {
            LevelManager.Instance.ShowAccuracyIndicator(ACCURACYINDICATOR.GOOD);
        }
        //Bad hit
        else
        {
            LevelManager.Instance.ShowAccuracyIndicator(ACCURACYINDICATOR.BAD);
        }
    }

    private void Miss()
    {
        //print($"Missed {inputIndex} note");
    }
}

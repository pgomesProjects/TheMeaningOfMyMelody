using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneController : MonoBehaviour
{
    [SerializeField] private NOTETYPE noteDirection;
    [SerializeField] private GameObject notePrefab;
    private List<NoteController> notes;
    private List<double> timeStamps;
    private int spawnIndex;

    // Start is called before the first frame update
    void Start()
    {
        notes = new List<NoteController>();
        timeStamps = new List<double>();
    }

    public void AddNote(double timeStamp)
    {
        timeStamps.Add(timeStamp);
    }

    // Update is called once per frame
    void Update()
    {
        if(spawnIndex < timeStamps.Count)
        {
            if(LevelManager.GetSongTime() >= timeStamps[spawnIndex] - LevelManager.Instance.noteTime)
            {
                var note = Instantiate(notePrefab, transform);
                NoteController currentNote = note.GetComponent<NoteController>();
                currentNote.SetNoteDirection(noteDirection);
                currentNote.SetAssignedTime((float)timeStamps[spawnIndex]);
                notes.Add(currentNote);
                spawnIndex++;
            }
        }
    }
}

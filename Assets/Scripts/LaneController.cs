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
        AddNote();
    }

    private void AddNote()
    {
        //Manually adding notes
        timeStamps.Add(10);
        timeStamps.Add(11);
        timeStamps.Add(11.5);
        timeStamps.Add(12);
        timeStamps.Add(13);
        timeStamps.Add(14);
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

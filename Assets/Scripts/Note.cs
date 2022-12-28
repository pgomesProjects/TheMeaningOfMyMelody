using UnityEngine;

[System.Serializable]
#pragma warning disable CS0660
#pragma warning disable CS0661
public class Note
#pragma warning restore CS0661
#pragma warning restore CS0660
{
    public int noteDirection;
    public double timeStamp;

    public Note()
    {
        noteDirection = 0;
        timeStamp = 0.0;
    }

    public Note(int noteDirection, double timeStamp)
    {
        this.noteDirection = noteDirection;
        this.timeStamp = timeStamp;
    }

    public static Note Vector2ToNote(Vector2 noteData)
    {
        return new Note((int)noteData[0], (double)noteData[1]);
    }

    public static bool operator ==(Note currentNote, Vector2 noteData)
    {
        if(currentNote.noteDirection == (int)noteData[0])
        {
            if(currentNote.timeStamp == (double)noteData[1])
            {
                return true;
            }
        }
        return false;
    }

    public static bool operator !=(Note currentNote, Vector2 noteData)
    {
        if (currentNote.noteDirection != (int)noteData[0])
        {
            return true;
        }

        if (currentNote.timeStamp != (double)noteData[1])
        {
            return true;
        }

        return false;
    }
}

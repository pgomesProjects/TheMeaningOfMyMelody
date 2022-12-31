using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEngine.Networking;

public static class ReadSongData
{
    private static string readFromFilePath;
    private static string directory;

    public static string GetSongDataFromFile(string currentSong, string fileName)
    {
        readFromFilePath = Application.streamingAssetsPath + "/Data/Charts/" + currentSong + "/" + fileName + ".json";
        directory = readFromFilePath;
        return directory;
    }

    public static void AddNotesToLanes(Note[] playerChart, ref LaneController[] lanes)
    {
        //Get all lines from file
        foreach (Note note in playerChart)
        {
            lanes[note.noteDirection].AddNote(note.timeStamp);
        }
    }
}

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

    public static void GetSongDataFromFile(string currentSong, string fileName)
    {
        readFromFilePath = Application.streamingAssetsPath + "/Data/Charts/" + currentSong + "/" + fileName + ".json";
        directory = readFromFilePath;
    }

    public static void AddNotesToLanes(ref LaneController[] lanes)
    {
        //Get all lines from text file
        List<string> allNotes = File.ReadAllLines(directory).ToList();

        foreach (string noteData in allNotes)
        {
            //Split the data using a comma delimiter
            string[] noteSplit = noteData.Split(',');

            lanes[int.Parse(noteSplit[0])].AddNote(double.Parse(noteSplit[1]));
        }
    }
}

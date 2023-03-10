using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using SFB;

public class SaveFile : MonoBehaviour
{
    public void OnDataSave()
    {
        //Get data from chart in readable form
        SongData songData = GetComponent<SongEditorManager>().SaveData();

        string data = JsonUtility.ToJson(songData);

        //Open the save file panel
        string path = StandaloneFileBrowser.SaveFilePanel("Save File", "", "song_data", "json");

        //If the user provided a directory to save to, write text to the new file
        if (!string.IsNullOrEmpty(path))
        {
            File.WriteAllText(path, data);
        }
    }
}

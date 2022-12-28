using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using SFB;

[System.Serializable]
public class ChartData
{
    public Note[] chart;

    public ChartData(List<Note> chart)
    {
        this.chart = chart.ToArray();
    }
}

public class SaveFile : MonoBehaviour
{
    public void OnDataSave()
    {
        //Get data from chart in readable form
        string data = "";

        ChartData opponentChart = new ChartData(GetComponent<SongEditorManager>().GetChartData(CHARTTYPE.OPPONENT));
        ChartData playerChart = new ChartData(GetComponent<SongEditorManager>().GetChartData(CHARTTYPE.PLAYER));

        data += JsonUtility.ToJson(opponentChart) + "\n";
        data += JsonUtility.ToJson(playerChart);

        //Open the save file panel
        string path = StandaloneFileBrowser.SaveFilePanel("Save File", "", "song_data", "json");

        //If the user provided a directory to save to, write text to the new file
        if (!string.IsNullOrEmpty(path))
        {
            File.WriteAllText(path, data);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongEditorManager : MonoBehaviour
{
    private List<Vector2> chart;

    private void OnEnable()
    {
        chart = new List<Vector2>();
        //GetComponent<OpenFile>().OpenDataFile();
    }

    public void AddNoteToChart(Vector2 noteData)
    {
        chart.Add(noteData);
    }

    public string GetChartData()
    {
        string chartData = "";

        for (int i = 0; i < chart.Count; i++)
        {
            string newNote = "";
            newNote += chart[i].x + ",";

            Debug.Log("Note X Pos: " + chart[i].y);

            double timeStamp = (double)(Mathf.Floor((float)LevelManager.GetFullSongDuration()) / GetComponentInChildren<GridManager>().GetColumns() * (chart[i].y / 100f));
            newNote += timeStamp;

            //If not the last line being written, add a new line for it
            if (i < chart.Count - 1)
                newNote += "\n";

            chartData += newNote;
        }

        return chartData;
    }
}

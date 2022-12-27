using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SongEditorManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI songPos;
    [SerializeField] private TextMeshProUGUI beatSnap;
    private List<Vector2> chart;

    private float[] beatSnaps = {0.25f, 0.5f, 0.75f, 1f, 1.25f, 1.5f, 2f, 3f, 4f, 6f, 12f};
    private int currentSnapIndex;

    private void OnEnable()
    {
        chart = new List<Vector2>();
        currentSnapIndex = 3;
        UpdateSongPosText();
        //GetComponent<OpenFile>().OpenDataFile();
    }

    public void AddNoteToChart(Vector2 noteData)
    {
        chart.Add(noteData);
    }

    public void RemoveNoteFromChart(Vector2 noteData)
    {
        chart.Remove(noteData);
    }

    public bool NoteExistsInChart(Vector2 noteData)
    {
        foreach (var chart in chart)
        {
            Debug.Log(chart + " vs. " + noteData);
            if(chart == noteData)
                return true;
        }

        return false;
    }

    public int GetNoteIndex(Vector2 noteData) => chart.IndexOf(noteData);

    public string GetChartData()
    {
        string chartData = "";

        for (int i = 0; i < chart.Count; i++)
        {
            string newNote = "";
            newNote += chart[i].x + ",";

            double timeStamp = (double)(Mathf.Floor((float)LevelManager.GetFullSongDuration()) / GetComponentInChildren<GridManager>().GetColumns() * (chart[i].y / 100f));
            newNote += timeStamp;

            //If not the last line being written, add a new line for it
            if (i < chart.Count - 1)
                newNote += "\n";

            chartData += newNote;
        }

        return chartData;
    }

    public void IncreaseBeatSnap()
    {
        currentSnapIndex += 1;

        if (currentSnapIndex > beatSnaps.Length - 1)
            currentSnapIndex = 0;

        UpdateBeatSnap();
    }

    public void DecreaseBeatSnap()
    {
        currentSnapIndex -= 1;

        if(currentSnapIndex < 0)
            currentSnapIndex = beatSnaps.Length - 1;

        UpdateBeatSnap();
    }

    private void UpdateBeatSnap()
    {
        GetComponentInChildren<EditorGridEvents>().SetBeatSnap(beatSnaps[currentSnapIndex]);
        beatSnap.text = (16f * beatSnaps[currentSnapIndex]).ToString() + "th";
    }

    public void UpdateSongPosText()
    {
        float gridXPos = GetComponentInChildren<EditorGridEvents>().GetComponent<RectTransform>().localPosition.x;
        float gridWidth = GetComponentInChildren<EditorGridEvents>().GetComponent<RectTransform>().sizeDelta.x;

        double seconds = LevelManager.GetFullSongDuration() * -(gridXPos / gridWidth);

        songPos.text = (Mathf.Round((float)seconds * 100f) / 100f).ToString() + " sec / " + (Mathf.Round((float)LevelManager.GetFullSongDuration() * 100f) / 100f).ToString() + " sec";
    }
}

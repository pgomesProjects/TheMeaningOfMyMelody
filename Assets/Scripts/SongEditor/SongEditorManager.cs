using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SongEditorManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI songPos;
    [SerializeField] private TextMeshProUGUI beatSnap;
    private List<Vector2> opponentChart;
    private List<Vector2> playerChart;

    private float[] beatSnaps = {0.25f, 0.5f, 0.75f, 1f, 1.25f, 1.5f, 2f, 3f, 4f, 6f, 12f};
    private int currentSnapIndex = 3;

    private void OnEnable()
    {
        opponentChart = new List<Vector2>();
        playerChart = new List<Vector2>();
        currentSnapIndex = 3;
        UpdateSongPosText();
        //GetComponent<OpenFile>().OpenDataFile();
    }

    public void AddNoteToChart(CHARTTYPE chartType, Vector2 noteData)
    {
        switch (chartType)
        {
            case CHARTTYPE.OPPONENT:
                opponentChart.Add(noteData);
                break;
            case CHARTTYPE.PLAYER:
                playerChart.Add(noteData);
                break;
        }
    }

    public void RemoveNoteFromChart(CHARTTYPE chartType, Vector2 noteData)
    {
        switch (chartType)
        {
            case CHARTTYPE.OPPONENT:
                opponentChart.Remove(noteData);
                break;
            case CHARTTYPE.PLAYER:
                playerChart.Remove(noteData);
                break;
        }
    }

    public bool NoteExistsInChart(CHARTTYPE chartType, Vector2 noteData)
    {
        List<Vector2> chartCopy = new List<Vector2>();

        switch (chartType)
        {
            case CHARTTYPE.OPPONENT:
                chartCopy = opponentChart;
                break;
            case CHARTTYPE.PLAYER:
                chartCopy = playerChart;
                break;
        }

        foreach (var chart in chartCopy)
        {
            //Debug.Log(chart + " vs. " + noteData);
            if(chart == noteData)
                return true;
        }

        return false;
    }

    public int GetNoteIndex(CHARTTYPE chartType, Vector2 noteData)
    {
        switch (chartType)
        {
            case CHARTTYPE.OPPONENT:
                return opponentChart.IndexOf(noteData);
            case CHARTTYPE.PLAYER:
                return playerChart.IndexOf(noteData);
        }

        return -1;
    }

    public string GetChartData(CHARTTYPE chartType)
    {
        List<Vector2> chartCopy = new List<Vector2>();

        switch (chartType)
        {
            case CHARTTYPE.OPPONENT:
                chartCopy = opponentChart;
                break;
            case CHARTTYPE.PLAYER:
                chartCopy = playerChart;
                break;
        }

        string chartData = "";

        for (int i = 0; i < chartCopy.Count; i++)
        {
            string newNote = "";
            newNote += chartCopy[i].x + ",";

            double timeStamp = (double)(Mathf.Floor((float)LevelManager.GetFullSongDuration()) / GetComponentInChildren<GridManager>().GetRows() * (playerChart[i].y / 100f));
            newNote += timeStamp;

            //If not the last line being written, add a new line for it
            if (i < chartCopy.Count - 1)
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
        beatSnap.text = (16f * beatSnaps[currentSnapIndex]).ToString() + "th";
    }

    public float GetPositionSnap() => beatSnaps[currentSnapIndex];
    public void UpdateSongPosText()
    {
        float gridYPos = GetComponentInChildren<EditorGridEvents>().GetComponent<RectTransform>().localPosition.y;
        float gridHeight = GetComponentInChildren<EditorGridEvents>().GetComponent<RectTransform>().sizeDelta.y;

        double seconds = Mathf.Round((float)(LevelManager.GetFullSongDuration() * (gridYPos / gridHeight)) * 100f) / 100f;

        double totalTime = Mathf.Round((float)LevelManager.GetFullSongDuration() * 100f) / 100f;

        songPos.text = seconds + " sec / " + totalTime + " sec";
    }

    public List<Vector2> GetOpponentChart() => opponentChart;
    public List<Vector2> GetPlayerChart() => playerChart;
}

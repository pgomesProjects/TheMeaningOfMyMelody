using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SongEditorManager : MonoBehaviour
{
    [SerializeField] private RectTransform strumTransform;
    [SerializeField] private TextMeshProUGUI songPos;
    [SerializeField] private TextMeshProUGUI sectionPos;
    [SerializeField] private TextMeshProUGUI beatSnap;
    private List<Vector2> opponentChart;
    private List<Vector2> playerChart;

    private float[] beatSnaps = {0.25f, 0.5f, 0.75f, 1f, 1.25f, 1.5f, 2f, 3f, 4f, 6f, 12f};
    private int currentSnapIndex = 3;

    private int currentSection = 0;
    private double currentStep = 0;

    private Vector3 strumDefaultTransform;
    private float strumMaxPosition = 31.7f;
    private float strumSnapValue;

    private void Start()
    {
        strumDefaultTransform = strumTransform.anchoredPosition;
        strumSnapValue = (strumDefaultTransform.y - strumMaxPosition) / 16f;
    }

    private void OnEnable()
    {
        opponentChart = new List<Vector2>();
        playerChart = new List<Vector2>();
        currentSnapIndex = 3;
        UpdateSongPosText(null);
        UpdateSection();
        //GetComponent<OpenFile>().OpenDataFile();
    }

    public void AddNoteToChart(CHARTTYPE chartType, Vector2 noteData)
    {
        switch (chartType)
        {
            case CHARTTYPE.OPPONENT:
                //Debug.Log("Adding Note: " + noteData);
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
            Debug.Log(chart.ToString("F0") + " vs. " + noteData.ToString("F0"));
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

    public List<Note> GetChartData(CHARTTYPE chartType)
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

        List<Note> chartData = new List<Note>();

        for (int i = 0; i < chartCopy.Count; i++)
        {
            int notePosition = (int)chartCopy[i].x;
            double timeStamp = (double)(Mathf.Floor((float)LevelManager.GetFullSongDuration()) / GetComponentInChildren<GridManager>().GetRows() * ((double)chartCopy[i].y / 100f));

            Note newNote = new Note(notePosition, -timeStamp);

            chartData.Add(newNote);
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

    public void ChangeSection(int increment)
    {
        switch (increment)
        {
            case -1:
                if(currentSection > 1)
                {
                    strumTransform.anchoredPosition = strumDefaultTransform;
                    currentSection += increment;
                }
                break;
            case 1:
                if (currentSection < GetMaxSections())
                {
                    strumTransform.anchoredPosition = strumDefaultTransform;
                    currentSection += increment;
                }
                break;
        }

        UpdateAllGrids(new Vector3(0, 1600 * (currentSection - 1), 0));
        UpdateSection();
    }

    private int CheckForCurrentSection()
    {
        EditorGridEvents grid = GetComponentInChildren<EditorGridEvents>();
        if (grid != null)
        {
            return (int)(grid.GetComponent<RectTransform>().localPosition.y / 1600f) + 1;
        }

        return 0;
    }

    private int GetMaxSections()
    {
        EditorGridEvents grid = GetComponentInChildren<EditorGridEvents>();
        if (grid != null)
        {
            return (int)(grid.GetComponent<RectTransform>().sizeDelta.y / 1600f);
        }

        return 0;
    }

    private void UpdateSection()
    {
        sectionPos.text = "Section: " + currentSection;
    }

    public void SnapStrumPosition(int increment)
    {
        switch (increment)
        {
            case -1:
                if (currentStep > 0)
                {
                    currentStep += increment;
                }
                else
                {
                    if (currentSection > 1)
                    {
                        currentStep = 15;
                        ChangeSection(-1);
                    }
                }
                break;
            case 1:
                if (currentStep < 15)
                {
                    currentStep += increment;
                }
                else
                {
                    if (currentSection < GetMaxSections())
                    {
                        currentStep = 0;
                        ChangeSection(1);
                    }
                }
                break;
        }

        strumTransform.anchoredPosition = new Vector3(strumDefaultTransform.x, strumDefaultTransform.y - (strumSnapValue * (float)currentStep), strumDefaultTransform.z);
        UpdateSongPosText(GetComponentInChildren<EditorGridEvents>().GetComponent<RectTransform>());
    }

    public float GetPositionSnap() => beatSnaps[currentSnapIndex];
    public void UpdateSongPosText(RectTransform updatedTransform)
    {
        if(updatedTransform == null)
        {
            double totalTime = Mathf.Round((float)LevelManager.GetFullSongDuration() * 100f) / 100f;

            songPos.text = "0.00 sec / " + totalTime.ToString("F2") + " sec";
        }
        else
        {
            UpdateAllGrids(updatedTransform.localPosition);
            currentSection = CheckForCurrentSection();
            UpdateSection();

            float gridYPos = updatedTransform.localPosition.y + (100f * (float)currentStep);
            float gridHeight = updatedTransform.sizeDelta.y;

            double seconds = Mathf.Round((float)(LevelManager.GetFullSongDuration() * (gridYPos / gridHeight)) * 100f) / 100f;

            double totalTime = Mathf.Round((float)LevelManager.GetFullSongDuration() * 100f) / 100f;

            if (seconds > totalTime)
                seconds = totalTime;

            songPos.text = seconds.ToString("F2") + " sec / " + totalTime.ToString("F2") + " sec";
        }
    }

    private void UpdateAllGrids(Vector3 newPos)
    {
        EditorGridEvents[] allGrids = GetComponentsInChildren<EditorGridEvents>();

        foreach (var grid in allGrids)
            grid.GetComponent<RectTransform>().localPosition = newPos;
    }
}

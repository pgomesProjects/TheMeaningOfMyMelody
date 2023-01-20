using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class SongEditorManager : MonoBehaviour
{
    private SongData songData;

    [SerializeField] private RectTransform strumTransform;
    [SerializeField] private TextMeshProUGUI songPos;
    [SerializeField] private TextMeshProUGUI sectionPos;
    [SerializeField] private TextMeshProUGUI beatSnap;
    private List<Vector2> opponentChart;
    private List<Vector2> playerChart;

    private float[] beatSnaps = {0.25f, 0.5f, 0.75f, 1f, 1.25f, 1.5f, 2f, 3f, 4f, 6f, 12f};
    private int currentSnapIndex = 3;

    private int currentSection = 1;
    private double currentStep = 0;

    private Vector3 strumDefaultTransform;
    private float strumMaxPosition = 31.7f;
    private float strumSnapValue;

    private float beatChartValue;

    private void Start()
    {
        strumDefaultTransform = strumTransform.anchoredPosition;
        strumSnapValue = (strumDefaultTransform.y - strumMaxPosition) / 16f;
        beatChartValue = (strumDefaultTransform.y - strumMaxPosition) / 4f;
    }

    private void OnEnable()
    {
        songData = new SongData();
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

    private List<Note> GetChartData(CHARTTYPE chartType)
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
            double timeStamp = (double)(float)LevelManager.GetFullSongDuration() / GetComponentInChildren<GridManager>().GetRows() * ((double)chartCopy[i].y / 100f);

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
                    currentStep = 0;
                }
                break;
            case 1:
                if (currentSection < GetMaxSections())
                {
                    strumTransform.anchoredPosition = strumDefaultTransform;
                    currentSection += increment;
                    currentStep = 0;
                }
                break;
        }

        UpdateAllGrids(new Vector3(0, 1600 * (currentSection - 1), 0));
        UpdateSection();
        UpdateSongPosText(GetComponentInChildren<EditorGridEvents>().GetComponent<RectTransform>());
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

    public void ChangeStrumPosition(float increment)
    {
        Vector3 currentPosition = strumTransform.anchoredPosition;
        currentPosition.y += increment;

        double nextStep = NextBeatStep(currentPosition);

        if (nextStep < 0 && currentSection > 1)
        {
            Debug.Log("Go To Previous Section");
            currentStep = 15;
            ChangeSection(-1);
            strumTransform.anchoredPosition = new Vector3(strumDefaultTransform.x, strumDefaultTransform.y - (strumSnapValue * (float)currentStep), strumDefaultTransform.z);
            currentStep = UpdateCurrentStep();
            UpdateSongPosText(GetComponentInChildren<EditorGridEvents>().GetComponent<RectTransform>());
            return;
        }
        else if(nextStep >= 16 && currentSection < GetMaxSections())
        {
            Debug.Log("Go To Next Section");
            currentStep = 0;
            ChangeSection(1);
            strumTransform.anchoredPosition = new Vector3(strumDefaultTransform.x, strumDefaultTransform.y - (strumSnapValue * (float)currentStep), strumDefaultTransform.z);
            currentStep = UpdateCurrentStep();
            UpdateSongPosText(GetComponentInChildren<EditorGridEvents>().GetComponent<RectTransform>());
            return;
        }

        if ((nextStep < 0 && currentSection <= 1) || (nextStep >= 16 && currentSection >= GetMaxSections()))
            return;

        strumTransform.anchoredPosition = currentPosition;
        currentStep = UpdateCurrentStep();
        UpdateSongPosText(GetComponentInChildren<EditorGridEvents>().GetComponent<RectTransform>());
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

    private double UpdateCurrentStep()
    {
        float currentYPos = strumDefaultTransform.y - strumTransform.anchoredPosition.y;
        Debug.Log("Step: " + (double)currentYPos / strumSnapValue);
        return (double)currentYPos / strumSnapValue;
    }

    private double NextBeatStep(Vector3 nextStepPos)
    {
        float currentYPos = strumDefaultTransform.y - nextStepPos.y;
        Debug.Log("Step: " + (double)currentYPos / strumSnapValue);
        return (double)currentYPos / strumSnapValue;
    }

    public float GetBeatChartValue() => beatChartValue;
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

    #region SAVEDATA
    public void SaveChartBPM(string chartBPM)
    {
        songData.chartBPM = int.Parse(chartBPM);
    }

    public void SaveOffset(string offset)
    {
        songData.offset = int.Parse(offset);
    }

    public void SaveScrollSpeed(string scrollSpeed)
    {
        songData.scrollSpeed = double.Parse(scrollSpeed);
    }

    public void SaveInstVolume(string instVolume)
    {
        songData.instVolume = double.Parse(instVolume);
    }

    public void SaveVocalsVolume(string vocalsVolume)
    {
        songData.vocalsVolume = double.Parse(vocalsVolume);
    }
    
    public void ClearNotes()
    {
        foreach(var grid in GetComponentsInChildren<EditorGridEvents>())
        {
            if(grid.GetChartType() != CHARTTYPE.EVENTS)
                grid.ClearNoteObjects();
        }

        playerChart.Clear();
        opponentChart.Clear();
    }

    public SongData SaveData()
    {
        songData.opponentChart = GetChartData(CHARTTYPE.OPPONENT).OrderBy(note => note.timeStamp).ToArray();
        songData.playerChart = GetChartData(CHARTTYPE.PLAYER).OrderBy(note => note.timeStamp).ToArray();
        return songData;
    }
    #endregion
}

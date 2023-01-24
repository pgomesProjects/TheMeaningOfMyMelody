using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Transform arrowHolder;
    [SerializeField] private Transform laneHolder;
    [SerializeField] private Transform accuracyIndicatorParent;
    [SerializeField] private AccuracyIndicatorController accuracyIndicator;

    public static LevelManager Instance;

    private bool isPaused;
    private bool inSongEditor;
    public string CurrentSong;
    private AudioSource currentSongAudioSource;

    [SerializeField] private TextMeshProUGUI songNameText;
    [SerializeField] private TextMeshProUGUI creditsText;

    [SerializeField] private LaneController[] opponentLanes;
    [SerializeField] private LaneController[] playerLanes;

    [Header("Chart Colors")]
    public Color leftNote;
    public Color downNote;
    public Color upNote;
    public Color rightNote;

    public float songDelaySeconds;
    public float marginOfError = 0.1f;
    public float goodHitMargin = 0.75f;
    public float greatHitMargin = 0.5f;
    public float perfectHitMargin = 0.25f;
    public float inputDelayMilliseconds;

    private float defaultNoteTime = 1.47f;

    private SongData currentSongData;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;
        inSongEditor = false;
        UpdateBoardSettings();
        SongSetup();
    }

    private void UpdateBoardSettings()
    {
        if (GameSettings.downScrollMultiplier == -1)
        {
            //Flip the position of the arrows and the lanes
            Vector3 newArrowPos = arrowHolder.position;
            newArrowPos.y = -newArrowPos.y;
            arrowHolder.position = newArrowPos;

            Vector3 newLanePos = laneHolder.position;
            newLanePos.y = -newLanePos.y;
            laneHolder.position = newLanePos;
        }
    }

    private void SongSetup()
    {
        currentSongAudioSource = FindObjectOfType<AudioManager>().GetSoundAudioSource(CurrentSong + "Inst");
        string directory = ReadSongData.GetSongDataFromFile(CurrentSong, CurrentSong);
        GetFileData(directory);
        FindObjectOfType<SongTimer>().TimerSetup();
        songNameText.text = "<i>" + currentSongData.songName + "</i>";
        creditsText.text = "By: " + currentSongData.songCredits;
        Invoke(nameof(StartSong), songDelaySeconds);
    }

    private void GetFileData(string directory)
    {
        string rawData = File.ReadAllText(directory);
        currentSongData = JsonUtility.FromJson<SongData>(rawData);
    }

    private void StartSong()
    {
        ReadSongData.AddNotesToLanes(currentSongData.opponentChart, ref opponentLanes);
        ReadSongData.AddNotesToLanes(currentSongData.playerChart, ref playerLanes);

        FindObjectOfType<AudioManager>().Play(CurrentSong + "Inst", PlayerPrefs.GetFloat("BGMVolume", 0.5f) * (float)currentSongData.instVolume);
        FindObjectOfType<AudioManager>().Play(CurrentSong + "Voices", PlayerPrefs.GetFloat("BGMVolume", 0.5f) * (float)currentSongData.vocalsVolume);
    }

    public double GetScrollSpeedTime()
    {
        return defaultNoteTime / currentSongData.scrollSpeed;
    }

    public static double GetSongTime()
    {
        return (double)Instance.currentSongAudioSource.timeSamples / Instance.currentSongAudioSource.clip.frequency;
    }

    public static double GetFullSongDuration()
    {
        if (Instance != null && Instance.currentSongAudioSource != null && Instance.currentSongAudioSource.clip != null)
            return (double)Instance.currentSongAudioSource.clip.length;

        return 0.0;
    }

    public void ShowAccuracyIndicator(ACCURACYINDICATOR accuracy)
    {
        AccuracyIndicatorController currentIndicator = Instantiate(accuracyIndicator, accuracyIndicatorParent);
        currentIndicator.GetComponent<RectTransform>().localPosition = accuracyIndicator.GetComponent<RectTransform>().localPosition;
        currentIndicator.UpdateIndicator(accuracy);
    }

    public static bool IsSongPlaying()
    {
        if (Instance != null && Instance.currentSongAudioSource != null && Instance.currentSongAudioSource.clip != null)
            return Instance.currentSongAudioSource.isPlaying;
        return false;
    }
    public SongData GetSongData() => currentSongData;

    public Note[] GetOpponentChartData() => currentSongData.opponentChart;
    public Note[] GetPlayerChartData() => currentSongData.playerChart;

    public bool IsPaused() => isPaused;
    public void SetPaused(bool paused)
    {
        isPaused = paused;
    }
    public bool InSongEditor() => inSongEditor;
    public void SetInSongEditor(bool songEditor)
    {
        inSongEditor = songEditor;
    }
}

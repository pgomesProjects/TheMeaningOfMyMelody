using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Transform arrowHolder;
    [SerializeField] private Transform laneHolder;
    [SerializeField] private Transform accuracyIndicatorParent;
    [SerializeField] private AccuracyIndicatorController accuracyIndicator;

    public float beatTempo = 120f;
    public float scrollSpeed = 1f;

    public static LevelManager Instance;

    public string CurrentSong;
    private AudioSource currentSongAudioSource;

    [SerializeField] private LaneController[] opponentLanes;
    [SerializeField] private LaneController[] playerLanes;

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
        UpdateBoardSettings();
        SongSetup();
    }

    private void UpdateBoardSettings()
    {
        if(GameSettings.downScrollMultiplier == -1)
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
        currentSongAudioSource = FindObjectOfType<AudioManager>().GetSoundAudioSource(CurrentSong+"Inst");
        string directory = ReadSongData.GetSongDataFromFile(CurrentSong, CurrentSong);
        GetFileData(directory);
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

        FindObjectOfType<AudioManager>().Play(CurrentSong+"Inst", PlayerPrefs.GetFloat("BGMVolume", 0.5f));
        FindObjectOfType<AudioManager>().Play(CurrentSong+"Voices", PlayerPrefs.GetFloat("BGMVolume", 0.5f));
    }

    public double GetScrollSpeedTime()
    {
        return defaultNoteTime / scrollSpeed;
    }

    public static double GetSongTime()
    {
        return (double)Instance.currentSongAudioSource.timeSamples / Instance.currentSongAudioSource.clip.frequency;
    }

    public static double GetFullSongDuration()
    {
        if(Instance != null && Instance.currentSongAudioSource != null && Instance.currentSongAudioSource.clip != null)
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
    public Note[] GetOpponentChartData() => currentSongData.opponentChart;
    public Note[] GetPlayerChartData() => currentSongData.playerChart;
}

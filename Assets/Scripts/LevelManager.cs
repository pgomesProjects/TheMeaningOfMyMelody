using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Transform arrowHolder;
    [SerializeField] private Transform laneHolder;

    public float beatTempo = 120f;
    public float scrollSpeed = 1f;

    public static LevelManager Instance;

    public string CurrentSong;
    private AudioSource currentSongAudioSource;

    [SerializeField] private LaneController[] lanes;

    public float songDelaySeconds;
    public double marginOfErrorSeconds;
    public float inputDelayMilliseconds;

    private float defaultNoteTime = 1.47f;

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
        ReadSongData.GetSongDataFromFile(CurrentSong, CurrentSong);
        Invoke(nameof(StartSong), songDelaySeconds);
    }

    private void StartSong()
    {
        ReadSongData.AddNotesToLanes(ref lanes);

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

    public LaneController GetLaneAt(int index) => lanes[index];
}

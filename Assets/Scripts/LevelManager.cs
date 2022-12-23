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

    public float songDelaySeconds;
    public double marginOfErrorSeconds;
    public float inputDelayMilliseconds;

    public float noteTime;

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
        if(GameSettings.leftScrollMultiplier == -1)
        {
            //Flip the position of the arrows and the lanes
            Vector3 newArrowPos = arrowHolder.position;
            newArrowPos.x = -newArrowPos.x;
            arrowHolder.position = newArrowPos;

            Vector3 newLanePos = laneHolder.position;
            newLanePos.x = -newLanePos.x;
            laneHolder.position = newLanePos;
        }
    }

    private void SongSetup()
    {
        currentSongAudioSource = FindObjectOfType<AudioManager>().GetSoundAudioSource(CurrentSong+"Inst");
        Invoke(nameof(StartSong), songDelaySeconds);
    }

    private void StartSong()
    {
        FindObjectOfType<AudioManager>().Play(CurrentSong+"Inst", PlayerPrefs.GetFloat("BGMVolume", 0.5f));
        FindObjectOfType<AudioManager>().Play(CurrentSong+"Voices", PlayerPrefs.GetFloat("BGMVolume", 0.5f));
    }

    public static double GetSongTime()
    {
        return (double)Instance.currentSongAudioSource.timeSamples / Instance.currentSongAudioSource.clip.frequency;
    }
}

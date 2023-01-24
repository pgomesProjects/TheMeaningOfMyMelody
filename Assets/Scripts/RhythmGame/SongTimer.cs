using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SongTimer : MonoBehaviour
{
    private Slider progressSlider;
    private TextMeshProUGUI timerText;
    private float currentSeconds;
    private float maxSeconds;
    private float percent;

    private bool songComplete = false;

    public void TimerSetup()
    {
        progressSlider = GetComponent<Slider>();
        timerText = GetComponentInChildren<TextMeshProUGUI>();

        currentSeconds = 0f;
        maxSeconds = Mathf.FloorToInt((float)LevelManager.GetFullSongDuration());

        UpdateTimerText();
    }

    // Update is called once per frame
    void Update()
    {
        if (!LevelManager.Instance.IsPaused() && !LevelManager.Instance.InSongEditor())
        {
            if (!songComplete)
            {
                currentSeconds += Time.deltaTime;
                percent = (currentSeconds / maxSeconds) * 100f;
                progressSlider.value = percent;

                UpdateTimerText();

                if(percent >= 100)
                {
                    songComplete = true;
                    OnComplete();
                }
            }
        }
    }

    private void UpdateTimerText()
    {
        timerText.text = DisplayTime(currentSeconds) + " / " + DisplayTime(maxSeconds);
    }

    private string DisplayTime(float currentTime)
    {
        string displayTime = "NaN";

        if(GetSeconds(currentTime) < 10)
        {
            displayTime = GetMinutes(currentTime) + ":0" + GetSeconds(currentTime);
        }
        else
        {
            displayTime = GetMinutes(currentTime) + ":" + GetSeconds(currentTime);
        }

        return displayTime;
    }

    private void OnComplete()
    {
        SceneManager.LoadScene("Menu");
    }

    private int GetMinutes(float currentTime) => Mathf.FloorToInt((float)currentTime / 60f);
    private int GetSeconds(float currentTime) => Mathf.FloorToInt((float)currentTime % 60f);
}

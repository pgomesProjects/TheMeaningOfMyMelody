using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSDisplay : MonoBehaviour
{
    private TextMeshProUGUI fpsDisplayText;
    private int avgFPS;

    private float updateTimer = 0.25f;
    private float currentTimer;

    // Start is called before the first frame update
    void Start()
    {
        fpsDisplayText = GetComponent<TextMeshProUGUI>();
        currentTimer = updateTimer;
    }

    // Update is called once per frame
    void Update()
    {
        avgFPS = (int)(1f / Time.unscaledDeltaTime);

        //Update the FPS counter every interval
        if(currentTimer >= updateTimer)
        {
            fpsDisplayText.text = "FPS: " + avgFPS.ToString();
            currentTimer = 0f;
        }
        else
            currentTimer += Time.unscaledDeltaTime;
    }
}

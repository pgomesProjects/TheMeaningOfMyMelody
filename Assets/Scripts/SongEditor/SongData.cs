using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SongData
{
    public string songName;
    public string songCredits;

    public int chartBPM;
    public int offset;
    public double scrollSpeed;
    public double instVolume;
    public double vocalsVolume;
    
    public Note[] opponentChart;
    public Note[] playerChart;
}

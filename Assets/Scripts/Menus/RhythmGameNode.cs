using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Song", menuName = "Rhythm Game Node")]
public class RhythmGameNode : StoryNode
{
    public string songName;
    public string setting;
}

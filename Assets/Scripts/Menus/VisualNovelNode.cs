using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Cutscene", menuName = "Visual Novel Node")]
public class VisualNovelNode : StoryNode
{
    public TextAsset visualNovelScript;
    public TextAsset visualNovelEventsScript;
}

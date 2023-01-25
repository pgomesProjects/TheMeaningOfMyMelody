using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StoryNode : ScriptableObject
{
    public string header;
    public string nodeTitle;

    public Sprite graphic;

    public bool hasViewed;
    public bool hasUnlocked;

    public bool IsEqual(StoryNode story)
    {
        if (story.header == header && story.nodeTitle == nodeTitle && story.graphic == graphic)
            return true;

        return false;
    }
}

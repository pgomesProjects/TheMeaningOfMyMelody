using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    private static SaveData _inst;

    public static SaveData Instance
    {
        get
        {
            if (_inst == null)
                _inst = new SaveData();

            return _inst;
        }
        set
        {
            _inst = value;
        }
    }

    public StoryTree story = new StoryTree();
}

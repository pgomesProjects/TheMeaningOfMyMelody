using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public enum EventType { ChangeEmotion, ShowSubtitle, FocusCamera }

[System.Serializable]
public class EventNote
{
    public EventType eventType;
    public object valueOne;
    public object valueTwo;
}

public class EventsInputField : MonoBehaviour
{
    private TMP_Dropdown eventDropdown;

    // Start is called before the first frame update
    void Start()
    {
        eventDropdown = GetComponent<TMP_Dropdown>();
        AddOptions();
    }

    private void AddOptions()
    {
        eventDropdown.ClearOptions();

        List<string> eventNames = Enum.GetNames(typeof(EventType)).ToList<string>();
        eventDropdown.AddOptions(eventNames);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

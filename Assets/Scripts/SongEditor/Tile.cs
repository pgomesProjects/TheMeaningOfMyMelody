using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public class Tile
{
    public string name;

    public AudioClip clip;

    [Range(.1f, 3f)]
    public float pitch = 1;

    public bool loop;

    public SOUNDTYPE soundType;

    [HideInInspector]
    public AudioSource source;

}

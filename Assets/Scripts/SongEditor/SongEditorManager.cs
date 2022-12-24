using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongEditorManager : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<OpenFile>().OpenDataFile();
    }
}

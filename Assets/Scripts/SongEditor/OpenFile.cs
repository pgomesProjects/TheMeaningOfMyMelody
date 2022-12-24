using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFB;
using UnityEngine.Networking;

public class OpenFile : MonoBehaviour
{
    private string fileData;

    public void OpenDataFile()
    {
        //Open file with Native Standalone File Browser
        string[] paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", "json", false);

        //If there is a file being loaded, open it
        if(paths.Length > 0)
        {
            StartCoroutine(OutputRoutineOpen(new System.Uri(paths[0]).AbsoluteUri));
        }
    }

    private IEnumerator OutputRoutineOpen(string url)
    {
        //Get the file
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        //If the file was not loaded successfully, print an error
        if(www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogWarning("File Could Not Be Loaded: " + url);
        }

        //If the file was loaded successfully, store the data for use
        else
        {
            Debug.Log("File Loaded: " + url);
            fileData = www.downloadHandler.text;
            Debug.Log("Data: " + fileData);
        }
    }
}

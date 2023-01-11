using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InputFieldChecker : MonoBehaviour
{
    public enum INPUTTYPE { ChartBPM, Offset, ScrollSpeed, InstrumentalVolume, VocalsVolume }

    [SerializeField] private INPUTTYPE inputType;
    [SerializeField] private double defaultValue = 0;
    [SerializeField] private double minimumValue = 0;
    [SerializeField] private double maximumValue = 1;
    [SerializeField] private int decimalPrecision = 0;
    private double currentValue;

    private TMP_InputField inputField;

    // Start is called before the first frame update
    void Start()
    {
        inputField = GetComponent<TMP_InputField>();
        inputField.text = defaultValue.ToString();
        if (LevelManager.Instance != null)
        {
            SetInputFields();
        }
        inputField.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
    }

    private void SetInputFields()
    {
        switch (inputType)
        {
            case INPUTTYPE.ChartBPM:
                inputField.text = LevelManager.Instance.GetSongData().chartBPM.ToString();
                FindObjectOfType<SongEditorManager>().SaveChartBPM(inputField.text);
                break;
            case INPUTTYPE.Offset:
                inputField.text = LevelManager.Instance.GetSongData().offset.ToString();
                FindObjectOfType<SongEditorManager>().SaveOffset(inputField.text);
                break;
            case INPUTTYPE.ScrollSpeed:
                inputField.text = LevelManager.Instance.GetSongData().scrollSpeed.ToString();
                FindObjectOfType<SongEditorManager>().SaveScrollSpeed(inputField.text);
                break;
            case INPUTTYPE.InstrumentalVolume:
                inputField.text = LevelManager.Instance.GetSongData().instVolume.ToString();
                FindObjectOfType<SongEditorManager>().SaveInstVolume(inputField.text);
                break;
            case INPUTTYPE.VocalsVolume:
                inputField.text = LevelManager.Instance.GetSongData().vocalsVolume.ToString();
                FindObjectOfType<SongEditorManager>().SaveVocalsVolume(inputField.text);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void ValueChangeCheck()
    {
        if (inputField.text.Length > 0)
        {
            string inputText = inputField.text.ToString();
            bool inputComplete = true;
            if (inputText[inputText.Length - 1] == '.' && decimalPrecision > 0)
            {
                inputText.Remove(inputText.Length - 1, 1);
                inputComplete = false;
            }
            currentValue = double.Parse(inputText);
            if (inputComplete)
            {
                int precision = decimalPrecision;
                currentValue = Mathf.Clamp((float)currentValue, (float)minimumValue, (float)maximumValue);

                //If the current value is an integer, display as an integer
                if (Mathf.Approximately((float)currentValue, Mathf.RoundToInt((float)currentValue)))
                    precision = 0;

                inputField.text = currentValue.ToString("F" + precision);
            }
        }
        else
        {
            currentValue = minimumValue;
            inputField.text = currentValue.ToString("F" + decimalPrecision);
        }
    }
}

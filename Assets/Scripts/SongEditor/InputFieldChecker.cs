using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InputFieldChecker : MonoBehaviour
{
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
        inputField.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
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

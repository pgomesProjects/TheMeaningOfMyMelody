using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EditorButton : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;

    public void IncrementValue(int value)
    {
        int currentValue = int.Parse(inputField.text);
        currentValue += value;

        inputField.text = currentValue.ToString();
        inputField.onEndEdit.Invoke(inputField.text);
    }

    public void IncrementValue(float value)
    {
        double currentValue = double.Parse(inputField.text);
        currentValue += (double)value;

        inputField.text = currentValue.ToString();
        inputField.onEndEdit.Invoke(inputField.text);
    }

    public void DecrementValue(int value)
    {
        int currentValue = int.Parse(inputField.text);
        currentValue -= value;

        inputField.text = currentValue.ToString();
        inputField.onEndEdit.Invoke(inputField.text);
    }

    public void DecrementValue(float value)
    {
        double currentValue = double.Parse(inputField.text);
        currentValue -= (float)value;

        inputField.text = currentValue.ToString();
        inputField.onEndEdit.Invoke(inputField.text);
    }
}

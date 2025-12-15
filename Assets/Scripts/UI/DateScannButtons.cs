using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DateScannButtons : MonoBehaviour
{
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private TMP_InputField dayField, monthField, yearField;

    void Start()
    {
        dayField.characterLimit = 2;
        monthField.characterLimit = 2;
        yearField.characterLimit = 4;

        dayField.onValidateInput = (string text, int charIndex, char addedChar) =>
        {
            return UITools.ValidateCharInclusie("0123456789", addedChar);
        };
        monthField.onValidateInput = (string text, int charIndex, char addedChar) =>
        {
            return UITools.ValidateCharInclusie("0123456789", addedChar);
        };
        yearField.onValidateInput = (string text, int charIndex, char addedChar) =>
        {
            return UITools.ValidateCharInclusie("0123456789", addedChar);
        };
    }
    public void SaveDate()
    {
        if(string.IsNullOrEmpty(dayField.text) || string.IsNullOrEmpty(monthField.text) || string.IsNullOrEmpty(yearField.text))
        {
            messageText.text = "Please fill in all date fields.";
            return;
        }

        int day = int.Parse(dayField.text), month = int.Parse(monthField.text), year = int.Parse(yearField.text);

        if (!DateTime.TryParseExact($"{year}-{month}-{day}", "yyyy-M-d", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
        {
            messageText.text = "Invalid date. Please enter a valid date.";
            return;
        }
        else
        {
            SerializableDate newDate = new SerializableDate(day, month, year, 0);
            ProductData.Instance.CurrentProduct.ExpirationDate = newDate;
            ProductData.Instance.SaveCurrentProduct = true;
            SceneManager.LoadScene(2);
        }
    }
}

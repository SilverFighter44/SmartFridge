using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public enum ScannerType
{
    Product,
    Date
}
public class ImageSender : MonoBehaviour
{
    // URL serwera
    private string url = "https://przepyszne.eu/upload/expire";

    [SerializeField] private ScannerType scannerType;
    [SerializeField] private TMP_InputField nameInput, dayInput, monthInput, yearInput;

    // Wstaw œcie¿kê wzglêdn¹ lub absolutn¹
    public string imagePath;

    private void Start()
    {
        switch(scannerType)
        {
            case ScannerType.Product:
                url = "https://przepyszne.eu/upload/food";
                break;
            case ScannerType.Date:
                url = "https://przepyszne.eu/upload/expire";
                break;
        }
    }
    public void SendImage( string imagePath )
    {
        this.imagePath = imagePath;
        StartCoroutine(UploadImage());
    }

    IEnumerator UploadImage()
    {
        // Sprawdzenie czy plik istnieje
        if (!File.Exists(imagePath))
        {
            Debug.LogError("Nie znaleziono pliku: " + imagePath);
            yield break;
        }

        // Wczytanie obrazu jako bajty
        byte[] imageBytes = File.ReadAllBytes(imagePath);

        // Multipart form-data
        WWWForm form = new WWWForm();
        form.AddBinaryData("image", imageBytes, Path.GetFileName(imagePath), "image/jpeg");

        UnityWebRequest request = UnityWebRequest.Post(url, form);

        // Wysy³anie
        yield return request.SendWebRequest();

        // Obs³uga b³êdów
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("B³¹d po³¹czenia: " + request.error);
        }
        else
        {
            // Odebrany JSON
            string json = request.downloadHandler.text;
            Debug.Log("OdpowiedŸ serwera: " + json);

            // Odczyt 
            try
            {
                switch(scannerType)
                {
                    case ScannerType.Product:
                        ProductResponse product = JsonUtility.FromJson<ProductResponse>(json);
                        nameInput.text = product.label;
                        Debug.Log("Znaleziony produkt: " + product.label + " (pewnoœæ: " + product.confidence + ")");
                        break;
                    case ScannerType.Date:
                        ExpireResponse data = JsonUtility.FromJson<ExpireResponse>(json); 
                        dayInput.text = data.date.Split('-')[2];
                        monthInput.text = data.date.Split('-')[1];
                        yearInput.text = data.date.Split('-')[0];
                        Debug.Log("Data wa¿noœci: " + data.date);
                        break;
                }

            }
            catch
            {
                Debug.LogWarning("Nie uda³o siê sparsowaæ JSON.");
            }
        }
    }
}


[System.Serializable]
public class ExpireResponse
{
    public string date; //y-m-d
}

[System.Serializable]
public class ProductResponse
{
    public string label;
    public float confidence;
}
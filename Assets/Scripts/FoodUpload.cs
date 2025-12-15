using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

public class FoodUpload : MonoBehaviour
{
    private string url = "https://przepyszne.eu/upload/food";

    void Start()
    {
        StartCoroutine(UploadFoodImage());
    }

    IEnumerator UploadFoodImage()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "1.jpg");

        byte[] imageData;

#if UNITY_ANDROID && !UNITY_EDITOR
        // Android wymaga UnityWebRequest do odczytu StreamingAssets
        using (UnityWebRequest imgRequest = UnityWebRequest.Get(path))
        {
            yield return imgRequest.SendWebRequest();

            if (imgRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Nie mo¿na wczytaæ obrazu: " + imgRequest.error);
                yield break;
            }

            imageData = imgRequest.downloadHandler.data;
        }
#else
        if (!File.Exists(path))
        {
            Debug.LogError("Nie znaleziono pliku: " + path);
            yield break;
        }

        imageData = File.ReadAllBytes(path);
#endif

        WWWForm form = new WWWForm();
        form.AddBinaryData("image", imageData, "image.jpg", "image/jpeg");

        using (UnityWebRequest request = UnityWebRequest.Post(url, form))
        {
            request.timeout = 30;
            request.SetRequestHeader("Accept", "application/json");

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"HTTP {request.responseCode}");
                Debug.LogError(request.error);
                Debug.LogError(request.downloadHandler.text);
                yield break;
            }

            Debug.Log("OdpowiedŸ serwera:");
            Debug.Log(request.downloadHandler.text);

            try
            {
                FoodResponse response =
                    JsonUtility.FromJson<FoodResponse>(request.downloadHandler.text);

                Debug.Log($"Produkt: {response.label}");
                Debug.Log($"Pewnoœæ: {response.confidence}");
            }
            catch
            {
                Debug.LogError("Nie uda³o siê sparsowaæ JSONa");
            }
        }
    }

    [System.Serializable]
    public class FoodResponse
    {
        public string label;
        public float confidence;
    }
}

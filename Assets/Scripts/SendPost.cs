using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class SendPost : MonoBehaviour
{
    private string url = "https://przepyszne.eu/upload/test";

    void Start()
    {
        StartCoroutine(SendJson());
    }

    IEnumerator SendJson()
    {
        // Tworzymy obiekt JSON
        var jsonData = "{\"text\":\"Czeœæ, serwerze z internetu!\"}";

        // Tworzymy request POST
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        // Ustawiamy nag³ówki
        request.SetRequestHeader("Content-Type", "application/json");

        // Wysy³amy
        yield return request.SendWebRequest();

        // Obs³uga odpowiedzi
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("OdpowiedŸ serwera: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("B³¹d: " + request.error);
            Debug.LogError("Treœæ b³êdu: " + request.downloadHandler.text);
        }
    }
}
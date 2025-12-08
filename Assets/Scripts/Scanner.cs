using UnityEngine;
using UnityEngine.UI;
using System.Drawing;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Networking;
using TMPro;

public class Scanner : MonoBehaviour
{
    [SerializeField] private RectTransform frame;
    [SerializeField] private float frameWidth, frameHeight;
    [SerializeField] private RawImage rawImage;
    [SerializeField] private ImageSender imageSender;
    private bool hasFrame = false;
    private WebCamTexture webcamTexture;

    void Awake()
    {
        hasFrame = (frame != null);
    }

    public Texture2D CaptureFrame()
    {
        if (webcamTexture == null || !webcamTexture.isPlaying)
            return null;

        Texture2D photo = new Texture2D(webcamTexture.width, webcamTexture.height, TextureFormat.RGB24, false);
        photo.SetPixels(webcamTexture.GetPixels());
        photo.Apply();

        return photo;
    }

    public void SavePNG()
    {
        Texture2D tex = CaptureFrame();
        if (tex == null) return;

        byte[] bytes = tex.EncodeToPNG();
        System.IO.File.WriteAllBytes(Application.persistentDataPath + "/photo.png", bytes);
        Debug.Log("Zapisano: " + Application.persistentDataPath + "/photo.png");
        imageSender.SendImage(Application.persistentDataPath + "/photo.png");
    }

    void Start()
    {
        // Pobierz dostêpne kamery
        WebCamDevice[] devices = WebCamTexture.devices;

        if (devices.Length > 0)
        {
            string cameraName = devices[0].name;
            webcamTexture = new WebCamTexture(cameraName);

            // Przypisz teksturê do RawImage
            RawImage rawImage = GetComponent<RawImage>();
            rawImage.texture = webcamTexture;

            // Dopasuj proporcje
            RectTransform rt = GetComponent<RectTransform>();
            float aspectRatio = (float)webcamTexture.width / webcamTexture.height;
            rt.sizeDelta = new Vector2(rt.sizeDelta.y * aspectRatio, rt.sizeDelta.y);

            webcamTexture.Play();

            rawImage.rectTransform.localEulerAngles = new Vector3(0, 0, -webcamTexture.videoRotationAngle);
            if (webcamTexture.videoVerticallyMirrored)
            {
                rawImage.rectTransform.localScale = new Vector3(1, -1, 1);
            }
            else
            {
                rawImage.rectTransform.localScale = Vector3.one;
            }
        }
        else
        {
            Debug.LogWarning("Brak dostêpnych kamer na urz¹dzeniu!");
        }
        if(hasFrame)
        {
            StartCoroutine(AdjustFrame());
        }
    }

    IEnumerator AdjustFrame()
    {
        yield return null;

        RectTransform rt = GetComponent<RectTransform>();

        // Ramka ma mieæ 60% wysokoœci i 60% szerokoœci RawImage
        float width = rt.rect.width * frameWidth;
        float height = rt.rect.height * frameHeight;

        frame.sizeDelta = new Vector2(width, height);
    }
    void OnDisable()
    {
        if (webcamTexture != null && webcamTexture.isPlaying)
        {
            webcamTexture.Stop();
        }
    }
}


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

            AspectRatioFitter arf = rawImage.GetComponent<AspectRatioFitter>();
            if (arf != null)
            {
                arf.aspectRatio = (float)webcamTexture.width / webcamTexture.height;
                arf.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
            }

            webcamTexture.Play();

            rawImage.rectTransform.localEulerAngles = new Vector3(0, 0, -webcamTexture.videoRotationAngle);
            rawImage.rectTransform.localScale = webcamTexture.videoVerticallyMirrored
                ? new Vector3(1, -1, 1)
                : Vector3.one;
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
        // Czekamy na wyrenderowanie RawImage i ustawienie tekstury
        yield return null;

        if (rawImage == null || frame == null || webcamTexture == null)
            yield break;

        RectTransform rawRect = rawImage.rectTransform;

        // Pobieramy aktualne wymiary RawImage w UI
        float rawWidth = rawRect.rect.width;
        float rawHeight = rawRect.rect.height;

        // Ustawiamy ramkê na 60% wymiarów RawImage
        float frameWidth = rawWidth * 0.5f;
        float frameHeight = rawHeight * 0.1f;

        frame.sizeDelta = new Vector2(frameWidth, frameHeight);

        // Wyœrodkowanie ramki wzglêdem RawImage
        frame.anchoredPosition = Vector2.zero;

        // Upewniamy siê, ¿e ramka dziedziczy obrót RawImage
        frame.localEulerAngles = Vector3.zero;
    }
    void OnDisable()
    {
        if (webcamTexture != null && webcamTexture.isPlaying)
        {
            webcamTexture.Stop();
        }
    }

    private void OnEnable()
    {
        if (webcamTexture != null && !webcamTexture.isPlaying)
        {
            webcamTexture.Play();
        }
    }
}


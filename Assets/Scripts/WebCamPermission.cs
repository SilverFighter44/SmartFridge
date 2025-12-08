using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebCamPermission : MonoBehaviour
{
    private IEnumerator Start()
    {
        if (!Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        }

        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            WebCamDevice[] devices = WebCamTexture.devices;
        }
    }
}


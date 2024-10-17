using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CameraAccess : MonoBehaviour
{
    public RawImage rawImage; // Arraste seu RawImage aqui no Inspector
    private WebCamTexture webcamTexture;

    void Start()
    {
        WebCamDevice[] devices = WebCamTexture.devices;

        if (devices.Length > 0)
        {   
            webcamTexture = new WebCamTexture(devices[0].name);
            rawImage.texture = webcamTexture;
            rawImage.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
            webcamTexture.Play();
        }
        else
        {
            Debug.LogError("Nenhuma câmera encontrada!");
        }
    }

    void OnDisable()
    {
        if (webcamTexture != null)
        {
            webcamTexture.Stop(); // Para a captura ao desativar o objeto
        }
    }
}

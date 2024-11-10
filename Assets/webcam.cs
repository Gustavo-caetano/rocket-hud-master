using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CameraAccess : MonoBehaviour
{
    public RawImage rawImage;
    private WebCamTexture webcamTexture;

    void Start()
    {
       UsarCamera();
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            UsarCamera();
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            try {
                UsarCamera(1);
            }catch {
                UsarCamera();
                ToastUtil.ShowToastError("Não foi possivel inicializar a outra camera");
            }
        }
        
    }
    void OnDisable()
    {
        if (webcamTexture != null)
        {
            webcamTexture.Stop();
        }
    }

    public void UsarCamera(int index = 0)
    {      
        OnDisable();
        WebCamDevice[] devices = WebCamTexture.devices;

        if (devices.Length > 0 && index < devices.Length)
        {   
            webcamTexture = new WebCamTexture(devices[index].name);
            rawImage.texture = webcamTexture;
            rawImage.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
            webcamTexture.Play();
        }
        else
        {
            Debug.LogError("Nenhuma câmera encontrada!");
        }
    }
}

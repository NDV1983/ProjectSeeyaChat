using UnityEngine;

public class CameraDeviceAdapaterView : MonoBehaviour
{
    public Camera MainCamera;
    public Camera UICamera;
    private int cameraSize = 12;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Detect Mobile
        if (Screen.width < Screen.height)
        {
            MainCamera.orthographicSize = cameraSize;
            UICamera.orthographicSize = cameraSize;
        }
    }
}


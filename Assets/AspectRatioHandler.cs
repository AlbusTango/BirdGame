using UnityEngine;

public class AspectRatioHandler : MonoBehaviour
{
    public float targetAspect = 9f / 16f; // portrait baseline
    public float baseOrthographicSize = 5f;

    Camera cam;

    void Awake()
    {
        cam = GetComponent<Camera>();
        cam.orthographic = true;
        Apply();
    }

    void Apply()
    {
        float screenAspect = (float)Screen.width / Screen.height;

        if (screenAspect < targetAspect)
        {
            // Narrow screen → increase vertical size
            float difference = targetAspect / screenAspect;
            cam.orthographicSize = baseOrthographicSize * difference;
        }
        else
        {
            // Wider or equal → normal size
            cam.orthographicSize = baseOrthographicSize;
        }
    }
}

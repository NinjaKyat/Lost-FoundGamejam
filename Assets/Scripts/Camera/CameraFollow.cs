using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    Camera camera;

    float targetZoom;
    float currentZoom;
    public float minZoom = 1;
    public float maxZoom = 10;
    float zoomChangeVelocity;

    private void Awake()
    {
        camera = GetComponent<Camera>();
        targetZoom = currentZoom = Mathf.InverseLerp(minZoom, maxZoom, camera.orthographicSize);
    }
    void Update()
    {
        targetZoom -= Input.mouseScrollDelta.y * 0.1f;
        targetZoom = Mathf.Clamp01(targetZoom);
        currentZoom = Mathf.SmoothDamp(currentZoom, targetZoom, ref zoomChangeVelocity, 0.1f);
        camera.orthographicSize = Mathf.Lerp(minZoom, maxZoom, currentZoom);
    }
}

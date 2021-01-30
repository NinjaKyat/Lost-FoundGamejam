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

    Vector2 positionChangeVelocity;

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

        if (CameraTarget.instances.Count == 0)
            return;

        var targetPosition = CameraTarget.instances[0].transform.position;
        var currentPosition = transform.position;
        currentPosition = Vector2.SmoothDamp(currentPosition, targetPosition, ref positionChangeVelocity, 0.2f);
        currentPosition.z = transform.position.z;
        transform.position = currentPosition;
    }
}

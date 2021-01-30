using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    public static List<CameraTarget> instances = new List<CameraTarget>();
    public static Action<CameraTarget> onCameraTargetAdded;
    public static Action<CameraTarget> onCameraTargetRemoved;

    public event Action<Vector2> onJump;

    void OnEnable()
    {
        instances.Add(this);
        onCameraTargetAdded?.Invoke(this);
    }

    void OnDisable()
    {
        instances.Remove(this);
        onCameraTargetRemoved?.Invoke(this);
    }

    public void JumpedBy(Vector2 offset)
    {
        onJump?.Invoke(offset);
    }
}

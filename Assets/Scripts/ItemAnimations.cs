using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAnimations : MonoBehaviour
{
    private Item item;
    public Vector2 Ground;
    public bool dropped = false;
    private float dropDuration = 1.5f;
    private float dropTimer;
    private Vector2 originalPosition;
    Vector3 velocity = Vector3.zero;
    float smoothTime = 0.15F;
    void Start()
    {
        item = GetComponent<Item>();
        Ground = item.transform.position;
        dropTimer = dropDuration;
    }
    
    void Update()
    {
        if (dropped && dropTimer < dropDuration)
        {
            dropTimer += Time.deltaTime;
            transform.position = Vector3.SmoothDamp(transform.position, Ground, ref velocity, smoothTime);
        }

        if (dropTimer > dropDuration)
            dropped = false;

        if (!dropped)
            dropTimer = 0;
    }

    public void ItemDropped(Vector2 ground)
    {
        dropped = true;
        Ground = ground;
        originalPosition = transform.position;
    }
}

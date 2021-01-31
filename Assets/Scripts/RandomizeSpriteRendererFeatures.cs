using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class RandomizeSpriteRendererFeatures : MonoBehaviour
{
    [SerializeField, HideInInspector] SpriteRenderer spriteRenderer;
    [SerializeField] TileObjectRepresentation tile;


    [SerializeField] bool flipX;
    [SerializeField] float tilt;
    [SerializeField] float scale;

    private void OnValidate()
    {
        if (!spriteRenderer)
            spriteRenderer = GetComponent<SpriteRenderer>();
        if (!tile)
            tile = GetComponentInParent<TileObjectRepresentation>();
    }
    private void Awake()
    {
        tile.onInitialized += HandleTileInitialized;
    }

    private void HandleTileInitialized()
    {
        var hash = tile.LocalGridPosition.x * 13;
        hash ^= 2147483647;
        hash ^= tile.LocalGridPosition.y * 17;
        hash ^= (transform.GetSiblingIndex() * 13);
        hash ^= 47581;

        if (flipX)
            spriteRenderer.flipX = (hash >> 5 & 1) == 1;
        hash ^= 47;
        var euler = transform.rotation.eulerAngles;
        euler.z += Mathf.Repeat(hash / 100f, tilt * 2) - tilt;
        transform.rotation = Quaternion.Euler(euler);
        hash ^= 47;
        transform.localScale *= 1 + Mathf.Repeat(hash / 100f, scale * 2) - scale;
    }
}

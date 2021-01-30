using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class RandomizeSpriteRendererFeatures : MonoBehaviour
{
    [SerializeField, HideInInspector] SpriteRenderer spriteRenderer;

    [SerializeField] bool flipX;
    [SerializeField] float tilt;
    [SerializeField] float scale;

    private void OnValidate()
    {
        if (!spriteRenderer)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        var hash = transform.position.GetHashCode();
        if (flipX)
            spriteRenderer.flipX = (hash & 1) == 1;
        hash ^= 47;
        var euler = transform.rotation.eulerAngles;
        euler.z += Mathf.Repeat(hash / 100f, tilt * 2) - tilt;
        transform.rotation = Quaternion.Euler(euler);
        hash ^= 47;
        transform.localScale *= 1 + Mathf.Repeat(hash / 100f, scale * 2) - scale;
    }
}

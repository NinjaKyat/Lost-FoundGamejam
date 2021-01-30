using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractibleObject : MonoBehaviour, IInteractible
{
    [SerializeField, HideInInspector] Collider2D collider;
    public Collider2D Collider => collider;

    private void OnValidate()
    {
        if (!collider)
            collider = GetComponent<Collider2D>();
    }

    public void Interact()
    {
        Destroy(gameObject);
    }
}

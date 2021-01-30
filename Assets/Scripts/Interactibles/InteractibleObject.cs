using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractibleObject : MonoBehaviour, IInteractible
{
    [SerializeField, HideInInspector] Collider2D _collider;
    public Collider2D Collider => _collider;

    private void OnValidate()
    {
        if (!_collider)
            _collider = GetComponent<Collider2D>();
    }

    public void Interact()
    {
        Destroy(gameObject);
    }
}

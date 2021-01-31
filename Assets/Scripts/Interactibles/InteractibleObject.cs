using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractibleObject : MonoBehaviour, IInteractible
{
    [SerializeField, HideInInspector] Collider2D _collider;
    public Collider2D Collider => _collider;

    [SerializeField, HideInInspector] TileObjectRepresentation tileObject;

    private void OnValidate()
    {
        if (!_collider)
            _collider = GetComponent<Collider2D>();
        if (!tileObject)
            tileObject = GetComponent<TileObjectRepresentation>();
    }

    public void Interact(Player interactingPlayer)
    {
        tileObject.Remove();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractibleObject : MonoBehaviour, IInteractible
{
    [SerializeField, HideInInspector] Collider2D _collider;
    public Collider2D Collider => _collider;

    public int health = 1;

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
        if (interactingPlayer.playerStats.GetStat("woodcutting") > 0)
            health -= 10;
        else health--;
        transform.localScale = transform.localScale / 1.2f;
        if (health < 0)
            tileObject.Remove();
    }
}

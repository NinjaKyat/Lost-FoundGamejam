using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeChildrenEnabled : MonoBehaviour
{
    [SerializeField] TileObjectRepresentation tile;

    private void OnValidate()
    {
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

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(hash % transform.childCount == child.GetSiblingIndex());
        }
    }
}

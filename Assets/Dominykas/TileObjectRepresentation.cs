using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileObjectRepresentation : MonoBehaviour
{
    public Vector2Int GridPosition { get; private set; }

    public void Spawn(Tile tile)
    {
        Instantiate(this, tile.WorldPosition, Quaternion.identity).Initialize(tile);
    }

    void Initialize(Tile tile)
    {
        GridPosition = tile.Position;
    }
}
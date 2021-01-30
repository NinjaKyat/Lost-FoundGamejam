using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileObjectRepresentation : MonoBehaviour
{
    public Vector2Int GridPosition { get; private set; }
    GameGrid grid;

    public void Spawn(Tile tile)
    {
        Instantiate(this, tile.WorldPosition, Quaternion.identity).Initialize(tile);
        Instantiate(this, tile.WorldPosition + tile.Grid.Size * Vector2Int.left, Quaternion.identity).Initialize(tile);
        Instantiate(this, tile.WorldPosition + tile.Grid.Size * Vector2Int.right, Quaternion.identity).Initialize(tile);
        Instantiate(this, tile.WorldPosition + tile.Grid.Size * Vector2Int.up, Quaternion.identity).Initialize(tile);
        Instantiate(this, tile.WorldPosition + tile.Grid.Size * Vector2Int.down, Quaternion.identity).Initialize(tile);
        Instantiate(this, tile.WorldPosition + tile.Grid.Size * (Vector2Int.left + Vector2Int.up), Quaternion.identity).Initialize(tile);
        Instantiate(this, tile.WorldPosition + tile.Grid.Size * (Vector2Int.left + Vector2Int.down), Quaternion.identity).Initialize(tile);
        Instantiate(this, tile.WorldPosition + tile.Grid.Size * (Vector2Int.right + Vector2Int.up), Quaternion.identity).Initialize(tile);
        Instantiate(this, tile.WorldPosition + tile.Grid.Size * (Vector2Int.left + Vector2Int.down), Quaternion.identity).Initialize(tile);
    }

    void Initialize(Tile tile)
    {
        GridPosition = tile.Position;
        grid = tile.Grid;
    }
}
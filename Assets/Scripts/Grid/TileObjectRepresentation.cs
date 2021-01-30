using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileObjectRepresentation : MonoBehaviour
{
    public Vector2Int GridPosition { get; private set; }
    GameGrid grid;

    public event Action onInitialized;

    public void Spawn(Tile tile)
    {
        var worldPosition = tile.WorldPosition;
        var gridPosition = tile.Position;
        Instantiate(this, worldPosition, Quaternion.identity).Initialize(tile);
        var gridSize = tile.Grid.Size;
        const int margin = 20;

        if (gridPosition.x > gridSize.x - margin)
            Instantiate(this, worldPosition + gridSize * Vector2Int.left, Quaternion.identity).Initialize(tile);
        if (gridPosition.x < margin)
            Instantiate(this, worldPosition + gridSize * Vector2Int.right, Quaternion.identity).Initialize(tile);
        if (gridPosition.y < margin)
            Instantiate(this, worldPosition + gridSize * Vector2Int.up, Quaternion.identity).Initialize(tile);
        if (gridPosition.y > gridSize.y - margin)
            Instantiate(this, worldPosition + gridSize * Vector2Int.down, Quaternion.identity).Initialize(tile);

        if (gridPosition.x > gridSize.x - margin && gridPosition.y < margin)
            Instantiate(this, worldPosition + gridSize * (Vector2Int.left + Vector2Int.up), Quaternion.identity).Initialize(tile);

        if (gridPosition.x > gridSize.x - margin && gridPosition.y > gridSize.y - margin)
            Instantiate(this, worldPosition + gridSize * (Vector2Int.left + Vector2Int.down), Quaternion.identity).Initialize(tile);

        if (gridPosition.x < margin && gridPosition.y < margin)
            Instantiate(this, worldPosition + gridSize * (Vector2Int.right + Vector2Int.up), Quaternion.identity).Initialize(tile);

        if (gridPosition.x < margin && gridPosition.y > gridSize.y - margin)
            Instantiate(this, worldPosition + gridSize * (Vector2Int.right + Vector2Int.down), Quaternion.identity).Initialize(tile);
    }

    void Initialize(Tile tile)
    {
        GridPosition = tile.Position;
        grid = tile.Grid;
        onInitialized?.Invoke();
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileObjectRepresentation : MonoBehaviour
{
    public Vector2Int LocalGridPosition => tile.LocalPosition;
    Tile tile;
    int indexInTile;
    TileObjectRepresentation original;

    public event Action onInitialized;

    public void Spawn(Tile tile, int indexInTile, List<GameObject> representations)
    {
        var worldPosition = tile.WorldPosition;
        var gridPosition = tile.LocalPosition;
        var original = Instantiate(this, worldPosition, Quaternion.identity);
        original.Initialize(tile, indexInTile, original, representations);
        var gridSize = tile.Grid.Size;
        const int margin = 20;

        if (gridPosition.x > gridSize.x - margin)
            Instantiate(this, worldPosition + gridSize * Vector2Int.left, Quaternion.identity).Initialize(tile, indexInTile, original, representations);
        if (gridPosition.x < margin)
            Instantiate(this, worldPosition + gridSize * Vector2Int.right, Quaternion.identity).Initialize(tile, indexInTile, original, representations);
        if (gridPosition.y < margin)
            Instantiate(this, worldPosition + gridSize * Vector2Int.up, Quaternion.identity).Initialize(tile, indexInTile, original, representations);
        if (gridPosition.y > gridSize.y - margin)
            Instantiate(this, worldPosition + gridSize * Vector2Int.down, Quaternion.identity).Initialize(tile, indexInTile, original, representations);

        if (gridPosition.x > gridSize.x - margin && gridPosition.y < margin)
            Instantiate(this, worldPosition + gridSize * (Vector2Int.left + Vector2Int.up), Quaternion.identity).Initialize(tile, indexInTile, original, representations);

        if (gridPosition.x > gridSize.x - margin && gridPosition.y > gridSize.y - margin)
            Instantiate(this, worldPosition + gridSize * (Vector2Int.left + Vector2Int.down), Quaternion.identity).Initialize(tile, indexInTile, original, representations);

        if (gridPosition.x < margin && gridPosition.y < margin)
            Instantiate(this, worldPosition + gridSize * (Vector2Int.right + Vector2Int.up), Quaternion.identity).Initialize(tile, indexInTile, original, representations);

        if (gridPosition.x < margin && gridPosition.y > gridSize.y - margin)
            Instantiate(this, worldPosition + gridSize * (Vector2Int.right + Vector2Int.down), Quaternion.identity).Initialize(tile, indexInTile, original, representations);
    }

    void Initialize(Tile tile, int indexInTile, TileObjectRepresentation original, List<GameObject> representations)
    {
        this.tile = tile;
        this.indexInTile = indexInTile;
        this.original = original;
        representations.Add(gameObject);

        onInitialized?.Invoke();
    }

    public void Remove()
    {
        tile.RemoveContent(indexInTile);
    }
}
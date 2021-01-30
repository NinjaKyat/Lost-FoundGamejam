using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public Vector2Int size;
    public TileObjectRepresentation treePrefab;

    Grid grid;

    void Awake()
    {
        grid = new Grid(size);
        grid.ForEachTile(PlaceTrees);
        grid.ForEachTile(PlaceObjects);
    }

    void PlaceTrees(Vector2Int position, ref Tile tile)
    {
        var scaledPos = new Vector2(position.x * 0.69f, position.y * 0.35f) * 0.2f;
        if (Mathf.PerlinNoise(scaledPos.x, scaledPos.y) > 0.5f)
            tile.contents.Add(new TileObject(TileObject.Type.Tree));
    }

    void PlaceObjects(Vector2Int position, ref Tile tile)
    {
        foreach (var contents in tile.contents)
        {
            if (contents is TileObject tileObject)
                if (tileObject.type == TileObject.Type.Tree)
                    treePrefab.Spawn(tile);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public Vector2Int size;
    public GameObject treePrefab;

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
            tile = new Tile() { type = Tile.Type.Tree };
    }

    void PlaceObjects(Vector2Int position, ref Tile tile)
    {
        if (tile.type == Tile.Type.Tree)
            Instantiate(treePrefab, (Vector2)position, Quaternion.identity);
    }
}

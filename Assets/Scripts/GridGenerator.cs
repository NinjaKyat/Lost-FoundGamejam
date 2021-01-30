using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public Vector2Int size;

    public TileObject.TileObjectRepresentations representations;

    GameGrid grid;

    void Awake()
    {
        grid = new GameGrid(size);
        grid.ForEachTile(PlaceGround);
        grid.ForEachTile(PlaceTrees);
        grid.ForEachTile(PlaceObjects);
    }

    void PlaceTrees(Vector2Int position, ref Tile tile)
    {
        if (tile.Contains(x => x is TileObject ob && ob.type == TileObject.Type.Grass))
        {
            var scale = new Vector2(0.69f, 0.35f) * 0.2f;
            if (SampleNoise(scale, Vector2.zero, position) > 0.5f)
                tile.AddContent(new TileObject(TileObject.Type.Tree));
        }
    }

    void PlaceGround(Vector2Int position, ref Tile tile)
    {
        var scaledPos = new Vector2(0.51f, 0.37f) * 0.2f;
        var offset = new Vector2(1337f, 0.69f);
        var placedTile = TileObject.Type.Grass;
        var noiseValue = SampleNoise(scaledPos, offset, position);
        if (noiseValue <= 0.2f)
            placedTile = TileObject.Type.Sand;
        //if (noiseValue <= 0.1f)
        //    placedTile = TileObject.Type.Water;


        tile.AddContent(new TileObject(placedTile));
    }

    void PlaceObjects(Vector2Int position, ref Tile tile)
    {
        foreach (var contents in tile.Contents)
        {
            if (contents is TileObject tileObject)
                tileObject.Spawn(representations, tile);
        }
    }

    float SampleNoise(Vector2 scale, Vector2 offset, Vector2Int gridPosition)
    {
        var gridSize = grid.Size;
        var relativePosition = new Vector2(gridPosition.x / (float)gridSize.x, gridPosition.y / (float)gridSize.y);
        var horizontalValue = Mathf.Lerp(PerlinNoise(gridPosition * scale + offset), PerlinNoise((gridPosition + new Vector2Int(gridSize.x, 0)) * scale + offset), relativePosition.x);
        var verticalValue = Mathf.Lerp(PerlinNoise(gridPosition * scale + offset), PerlinNoise((gridPosition + new Vector2Int(0,gridSize.y)) * scale + offset), relativePosition.y);

        return PerlinNoise(gridPosition * scale + offset);
    }

    float PerlinNoise(Vector2 sample)
    {
        return Mathf.PerlinNoise(sample.x, sample.y);
    }
}

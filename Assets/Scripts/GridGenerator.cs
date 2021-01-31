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
        grid.ForEachTile(PlaceRocks);
        grid.ForEachTile(PlaceTrees);
        grid.ForEachTile(PlaceEvents);
        grid.ForEachTile(PlaceObjects);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
            Advance();
    }

    public void Advance()
    {

    }

    void PlaceGround(Vector2Int position, ref Tile tile)
    {
        var scaledPos = new Vector2(0.51f, 0.37f) * 0.2f;
        var offset = new Vector2(1337f, 0.69f);
        var placedTile = TileObject.Type.Grass;
        var noiseValue = SampleNoise(scaledPos, offset, position);
        if (noiseValue <= 0.2f)
            placedTile = TileObject.Type.Sand;
        if (noiseValue <= 0.1f)
            placedTile = TileObject.Type.Water;

        tile.AddContent(new TileObject(placedTile));
        if (SampleNoise(scaledPos * 5, offset * 2f, position) > 0.9f)
            tile.AddContent(new TileEvent());
    }

    void PlaceRocks(Vector2Int position, ref Tile tile)
    {
        var scale = new Vector2(0.269f, 0.135f);
        if (SampleNoise(scale, Vector2.up * 123f, position) > 0.85f)
            tile.AddContent(new TileObject(TileObject.Type.Rock));
    }

    void PlaceTrees(Vector2Int position, ref Tile tile)
    {
        if (tile.Contains(x => x is TileObject ob && ob.type == TileObject.Type.Grass) && !tile.Contains(x => x is TileObject ob && ob.type == TileObject.Type.Rock))
        {
            var scale = new Vector2(0.69f, 0.35f) * 0.2f;
            if (SampleNoise(scale, Vector2.zero, position) > 0.5f)
            {
                var noise = SampleNoise(scale * 0.95f, Vector2.up * 0.1f, position);
                if (noise > 0.65f)
                {
                    if (noise < 0.7f)
                    {
                        tile.AddContent(new TileObject(TileObject.Type.Berries));
                        tile.AddContent(new TileEvent());
                    }
                    else
                    {
                        tile.AddContent(new TileObject(TileObject.Type.Bush));
                        if (SampleNoise(scale * 5, Vector2.zero, position) > 0.5f)
                            tile.AddContent(new TileEvent());
                    }
                }
                else
                {
                    tile.AddContent(new TileObject(TileObject.Type.Tree));
                    if (SampleNoise(scale * 5, Vector2.zero, position) > 0.8f)
                        tile.AddContent(new TileEvent());
                }
            }
        }
    }

    void PlaceEvents(Vector2Int position, ref Tile tile)
    {
        //if (position.x % 2 == position.y % 3)
            //tile.AddContent(new TileEvent());
    }

    void PlaceObjects(Vector2Int position, ref Tile tile)
    {
        tile.SpawnContents(representations);
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

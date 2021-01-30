using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public Vector2Int size;
    public TileObjectRepresentation treePrefab;
    public TileObjectRepresentation grassPrefab;
    public TileObjectRepresentation sandPrefab;

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
            var scaledPos = new Vector2(position.x * 0.69f, position.y * 0.35f) * 0.2f;
            if (Mathf.PerlinNoise(scaledPos.x, scaledPos.y) > 0.5f)
                tile.AddContent(new TileObject(TileObject.Type.Tree));
        }
    }

    void PlaceGround(Vector2Int position, ref Tile tile)
    {
        var scaledPos = new Vector2(position.x * 0.69f + 1337f, position.y * 0.35f + 0.69f) * 0.2f;
        var placedTile = TileObject.Type.Grass;
        var noiseValue = Mathf.PerlinNoise(scaledPos.x, scaledPos.y);
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
                switch (tileObject.type)
                {
                    case TileObject.Type.Water:
                        break;
                    case TileObject.Type.Sand:
                        sandPrefab.Spawn(tile);
                        break;
                    case TileObject.Type.Grass:
                        grassPrefab.Spawn(tile);
                        break;
                    case TileObject.Type.Tree:
                            treePrefab.Spawn(tile);
                        break;
                    default:
                        break;
                }
        }
    }
}

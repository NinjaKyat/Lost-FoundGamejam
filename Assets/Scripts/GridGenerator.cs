using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public Vector2Int size;

    public TileObject.TileObjectRepresentations representations;

    GameGrid grid;
    int generation = 0;

    [System.Serializable]
    public struct ThingsToSpawn
    {
        public GameObject go;
        public int count;
    }
    public ThingsToSpawn[] thingsToSpawnInFreeTiles; 

    void Awake()
    {
        grid = new GameGrid(size);
        grid.ForEachTile(PlaceGround);
        grid.ForEachTile(PlaceRocks);
        grid.ForEachTile(PlaceTrees);
        grid.ForEachTile(PlaceEvents);
        var freeTiles = new List<Tile>();
        grid.GetTiles(freeTiles, x => x is TileObject ob && (ob.type == TileObject.Type.Rock || ob.type == TileObject.Type.Water));
        foreach (var spawnable in thingsToSpawnInFreeTiles)
        {
            for (int i = 0; i < spawnable.count; i++)
            {
                if (freeTiles.Count > 0)
                {
                    var index = Random.Range(0, freeTiles.Count);
                    Instantiate(spawnable.go, freeTiles[index].WorldPosition, Quaternion.identity);
                    freeTiles.RemoveAt(index);
                }
            }
        }

        grid.ForEachTile(SpawnObjects);
        Advance();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
            Advance();
    }

    public void Advance()
    {
        generation++;
        for (int i = 0; i < 5; i++)
        {
            grid.ForEachTile(GrowBushes);
            grid.ForEachTile(GrowTrees);
        }
    }

    void GrowTrees(Vector2Int position, ref Tile tile)
    {
        if (!tile.Contains(IsGrass))
            return;

        var treeCount = tile.CountAround(IsTree, 1);
        if (treeCount > 5 && SampleNoise(Vector2.one * 3.598f, Vector2.one * 14.77f, tile.LocalPosition) > 0.8f)
        {
            var index = tile.IndexOfContent(IsTree);
            if (index > 0)
                tile.RemoveContentDownTo(index);
        }

        if (tile.Contains(IsBlocked))
            return;

        if (treeCount == 1 && SampleNoise(Vector2.one * 0.214f, Vector2.one * 44.2f, tile.LocalPosition) > 0.8f)
        {
            tile.AddContentAndSpawn(new TileObject(TileObject.Type.Tree), representations);
        }
    }

    void GrowBushes(Vector2Int position, ref Tile tile)
    {
        if (!tile.Contains(IsGrass))
            return;

        var bushCount = tile.CountAround(IsBush, 2);
        if (SampleNoise(Vector2.one * 0.4445f, Vector2.one * 67.3333f, tile.LocalPosition) > 0.8f)
        {

            var index = tile.IndexOfContent(IsBush);
            if (index > 0)
                tile.RemoveContentDownTo(index);
        }

        if (tile.Contains(IsBlocked))
            return;

        if (bushCount < 2 && SampleNoise(Vector2.one * 0.798f, Vector2.one * 123.132f, tile.LocalPosition) > 0.7f)
        {
            if (SampleNoise(Vector2.one * 0.7498f, Vector2.one * 1423.132f, tile.LocalPosition) > 0.5f)
            {
                tile.AddContentAndSpawn(new TileObject(TileObject.Type.Bush), representations);
            }
            else
            {
                tile.AddContentAndSpawn(new TileObject(TileObject.Type.Berries), representations);
                tile.AddContent(new TileEvent());
            }
        }
    }

    bool IsTree(ITileContent content)
    {
        return content is TileObject ob && ob.type == TileObject.Type.Tree;
    }

    bool IsBush(ITileContent content)
    {
        return content is TileObject ob && (ob.type == TileObject.Type.Bush || ob.type == TileObject.Type.Berries);
    }

    bool IsBlocked(ITileContent content)
    {
        return content is TileObject ob && (ob.type == TileObject.Type.Bush || ob.type == TileObject.Type.Rock || ob.type == TileObject.Type.Berries || ob.type == TileObject.Type.Tree);
    }

    bool IsGrass(ITileContent content)
    {
        return content is TileObject ob && ob.type == TileObject.Type.Grass;
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

    void SpawnObjects(Vector2Int position, ref Tile tile)
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
        return Mathf.PerlinNoise(sample.x + generation * 23.13f, sample.y + generation * 13.17f);
    }
}

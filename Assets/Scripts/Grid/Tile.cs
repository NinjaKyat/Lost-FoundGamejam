using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface ITileContent
{
}

public class TileEvent : ITileContent
{
    public GameEvent gameEvent;
    public TileEvent(GameEvent gameEvent)
    {
        this.gameEvent = gameEvent;
    }
    public TileEvent()
    { }

}

public class TileObject : ITileContent
{
    [Serializable]
    public struct TileObjectRepresentations
    {
        public TileObjectRepresentation treePrefab;
        public TileObjectRepresentation rockPrefab;
        public TileObjectRepresentation bushPrefab;
        public TileObjectRepresentation berriesPrefab;

        public TileObjectRepresentation grassPrefab;
        public TileObjectRepresentation sandPrefab;
        public TileObjectRepresentation waterPrefab;

        public TileObjectRepresentation GetPrefab(Type type)
        {
            switch (type)
            {
                case Type.Water:
                    return waterPrefab;
                case Type.Sand:
                    return sandPrefab;
                case Type.Grass:
                    return grassPrefab;
                case Type.Tree:
                    return treePrefab;
                case Type.Rock:
                    return rockPrefab;
                case Type.Bush:
                    return bushPrefab;
                case Type.Berries:
                    return berriesPrefab;
                default:
                    return null;
            }
        }
    }

    public enum Type
    {
        Water,
        Sand,
        Grass,
        Tree,
        Rock,
        Bush,
        Berries,
    }
    public Type type;
    List<GameObject> representations;

    public TileObject(Type type)
    {
        this.type = type;
        representations = new List<GameObject>();
    }

    public void Spawn(TileObjectRepresentations representationPrefabs, Tile tile, int indexInTile)
    {
        var prefab = representationPrefabs.GetPrefab(type);
        prefab.Spawn(tile, indexInTile, representations);
    }

    public void DestroyRepresentations()
    {
        foreach (var representation in representations)
        {
            GameObject.Destroy(representation);
        }
    }

    public override string ToString()
    {
        return type.ToString();
    }
}

public class Tile
{
    public GameGrid Grid { get; private set; }
    public Vector2Int LocalPosition { get; private set; }
    public Vector2 WorldPosition => Grid.LocalToWorldPosition(LocalPosition);

    List<ITileContent> contents;
    public IReadOnlyList<ITileContent> Contents => contents;

    public Tile(GameGrid grid, Vector2Int position)
    {
        Grid = grid;
        LocalPosition = position;
        contents = new List<ITileContent>();
    }

    public void AddContent(ITileContent newContents)
    {
        contents.Add(newContents);
    }

    internal void AddContentAndSpawn(ITileContent content, TileObject.TileObjectRepresentations representations)
    {
        contents.Add(content);
        if (content is TileObject tileObject)
            tileObject.Spawn(representations, this, contents.Count - 1);
    }

    public void RemoveTopContent()
    {
        contents.RemoveAt(contents.Count - 1);
    }

    public void RemoveContent(int index)
    {
        var tileObject = contents[index] as TileObject;
        if (tileObject != null)
        {
            tileObject.DestroyRepresentations();
        }
        contents.RemoveAt(index);
    }

    public void RemoveContentDownTo(int index)
    {
        for (int i = contents.Count - 1; i >= index; i--)
        {
            RemoveContent(index);
        }
    }

    public int IndexOfContent(Func<ITileContent, bool> searchFunc)
    {
        for (int i = contents.Count -1; i>= 0; i--)
        {
            if (searchFunc(contents[i]))
                return i;
        }
        return -1;
    }

    internal void SpawnContents(TileObject.TileObjectRepresentations representations)
    {
        for (int i = 0; i < contents.Count; i++)
        {
            var content = contents[i];
            if (content is TileObject tileObject)
                tileObject.Spawn(representations, this, i);
        }
    }

    public bool Contains(Func<ITileContent, bool> searchFunc)
    {
        foreach (var content in contents)
        {
            if (searchFunc.Invoke(content))
                return true;
        }
        return false;
    }

    public int CountAround(Func<ITileContent, bool> searchFunc, float distance)
    {
        var result = 0;
        for (int x = -Mathf.CeilToInt(distance); x < Mathf.CeilToInt(distance); x++)
        {
            for (int y = -Mathf.CeilToInt(distance); y < Mathf.CeilToInt(distance); y++)
            {
                var offset = new Vector2Int(x, y);
                if (offset.sqrMagnitude <= distance * distance)
                    if (Grid.GetTile(Grid.LocalToLocalWrappedPosition(LocalPosition + offset)).Contains(searchFunc))
                        result++;
            }
        }
        return result;
    }

    public GameEvent GetTopEventIfAvailable(Player player)
    {
        if (contents.Count == 0)
            return null;

        if (contents[contents.Count - 1] is TileEvent eventTile)
        {
            var gameEvent = eventTile.gameEvent;
            if (gameEvent == null)
            {
                var tag = "";
                for (int i = Contents.Count - 1; i >= 0; i--)
                {
                    if (contents[i] is TileObject tileObject)
                    {
                        tag = tileObject.type.ToString().ToLowerInvariant();
                        break;
                    }
                }
                gameEvent = EventMeister.GetRandomEvent(player.playerStats, tag);
            }
            RemoveTopContent();
            return gameEvent;
        }
        else
            return null;
    }

    public string GetString()
    {
        var s = "";
        s += LocalPosition ;
        foreach (var content in contents)
        {
            s += "\n" + content.ToString();
        }
        return s;
    }
}
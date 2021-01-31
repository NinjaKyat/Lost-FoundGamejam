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

}

public class TileObject : ITileContent
{
    [Serializable]
    public struct TileObjectRepresentations
    {
        public TileObjectRepresentation treePrefab;
        public TileObjectRepresentation rockPrefab;
        public TileObjectRepresentation bushPrefab;

        public TileObjectRepresentation grassPrefab;
        public TileObjectRepresentation sandPrefab;
        public TileObjectRepresentation waterPrefab;


        public TileObjectRepresentation GetPrefab(Type type)
        {
            switch (type)
            {
                case Type.Water:
                    return waterPrefab; //waterPrefab;
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
}

public class Tile
{
    public GameGrid Grid { get; private set; }
    public Vector2Int Position { get; private set; }
    public Vector2 WorldPosition => Grid.LocalToWorldPosition(Position);

    List<ITileContent> contents;
    public IReadOnlyList<ITileContent> Contents => contents;

    public Tile(GameGrid grid, Vector2Int position)
    {
        Grid = grid;
        Position = position;
        contents = new List<ITileContent>();
    }

    public void AddContent(ITileContent newContents)
    {
        contents.Add(newContents);
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

    public GameEvent GetTopEventIfAvailable(Player player)
    {
        if (contents.Count == 0)
            return null;

        if (contents[contents.Count - 1] is TileEvent eventTile)
        {
            var gameEvent = eventTile.gameEvent;
            if (gameEvent == null)
                gameEvent = EventMeister.GetRandomEvent(player.playerStats);
            RemoveTopContent();
            return gameEvent;
        }
        else
            return null;
    }
}
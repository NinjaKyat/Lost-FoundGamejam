using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface ITileContent
{
}

public struct TileEvent : ITileContent
{
    public GameEvent gameEvent;
}

public struct TileObject : ITileContent
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
    public TileObject(Type type)
    {
        this.type = type;
    }

    public void Spawn(TileObjectRepresentations representations, Tile tile)
    {
        var prefab = representations.GetPrefab(type);
        prefab.Spawn(tile);
    }
}

public struct Tile
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

    public bool Contains(Func<ITileContent, bool> searchFunc)
    {
        foreach (var content in contents)
        {
            if (searchFunc.Invoke(content))
                return true;
        }
        return false;
    }
}
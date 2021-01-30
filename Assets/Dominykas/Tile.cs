using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface ITileContents
{
}

public struct TileEvent : ITileContents
{
    public GameEvent gameEvent;
}

public struct TileObject : ITileContents
{
    public enum Type
    {
        Tree,
        Water,
        Ground,
    }
    public Type type;
    public TileObject(Type type)
    {
        this.type = type;
    }
}

public struct Tile
{
    public Grid Grid { get; private set; }
    public Vector2Int Position { get; private set; }
    public Vector2 WorldPosition => Position - Grid.Size / 2;

    public List<ITileContents> contents;

    public Tile(Grid grid, Vector2Int position)
    {
        Grid = grid;
        Position = position;
        contents = new List<ITileContents>();
    }
}
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
    public enum Type
    {
        Water,
        Sand,
        Grass,
        Tree,
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

    List<ITileContent> contents;
    public IReadOnlyList<ITileContent> Contents => contents;

    public Tile(Grid grid, Vector2Int position)
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
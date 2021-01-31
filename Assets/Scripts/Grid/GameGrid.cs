using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid
{
    public static GameGrid instance;

    Vector2Int size;
    public Vector2Int Size => size;

    Tile[,] data;

    public delegate void TileRef(Vector2Int position, ref Tile tile);

    public GameGrid(Vector2Int size)
    {
        this.size = size;
        data = new Tile[size.x, size.y];
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                data[x,y] = new Tile(this, new Vector2Int(x,y));
            }
        }
        if (instance != null)
        {
            Debug.LogError("Game Grid singleton already exists, but new one is created");
        }
        instance = this;
    }

    public void ForEachTile(TileRef action)
    {
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                action.Invoke(new Vector2Int(x, y), ref data[x, y]);
            }
        }
    }

    public Tile GetTile(Vector2Int localGridPosition)
    {
        return data[localGridPosition.x, localGridPosition.y];
    }

    public Vector2Int LocalToWorldPosition(Vector2Int localPosition)
    {
        return localPosition - Size / 2;
    }

    public bool WrapAround(ref Vector2 worldPosition)
    {
        var intPosition = Vector2Int.RoundToInt(worldPosition);
        var wrappedPosition = WorldToLocalWrappedPosition(worldPosition) - size / 2;
        if (intPosition != wrappedPosition)
        {
            worldPosition += wrappedPosition - intPosition;
            return true;
        }
        else
        {
            return false;
        }
    }

    public Vector2Int WorldToLocalWrappedPosition(Vector2 position)
    {
        var localIntPosition = Vector2Int.RoundToInt(position + size / 2);
        var newLocalIntPosition = localIntPosition;
        return new Vector2Int(((newLocalIntPosition.x % size.x) + size.x) % size.x, ((newLocalIntPosition.y % size.y) + size.y) % size.y);
    }
}

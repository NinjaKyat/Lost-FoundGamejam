using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    Vector2Int size;
    public Vector2Int Size => size;

    Tile[,] data;

    public delegate void TileRef(Vector2Int position, ref Tile tile);

    public Grid(Vector2Int size)
    {
        this.size = size;
        data = new Tile[size.x, size.y];
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

    public Tile GetTile(Vector2Int position)
    {
        var warpedPosition = new Vector2Int((position.x % size.x + size.x) % size.x, (position.y % size.y + size.y) % size.y);
        return data[warpedPosition.x, warpedPosition.y];
    }
}

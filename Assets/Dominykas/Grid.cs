using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    public Vector2Int size;
    public Tile[,] data;

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
}

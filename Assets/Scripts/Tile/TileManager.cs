using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class TileManager : Singleton<TileManager>
{
    public static Dictionary<int, Tile> tiles = new Dictionary<int, Tile>();
    public int[] tempId;
    public Tile[] tempTiles;

    private void Awake()
    {
        for (int i = 0; i < tempId.Length; i++)
        {
            tiles.Add(tempId[i], tempTiles[i]);
        }
    }
}

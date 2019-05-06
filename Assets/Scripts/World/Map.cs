using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public static class Map {

    public const int FRONT_MAP = 0;
    public const int BACK_MAP = 1;
    public static int[][,] map = new int[2][,];

    /// <summary>
    /// Check if preffered area exist 
    /// </summary>
    /// <param name="startPosX"></param>
    /// <param name="startPosY"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    public static bool CheckSubmapAvailability(int startPosX, int startPosY, int width, int height, MapLayer layer)
    {
        int[,] currentMap = GetMap(layer);

        if (currentMap == null)
            return false;

        if (currentMap.GetUpperBound(0) < startPosX + width - 1 ||
            currentMap.GetUpperBound(1) < startPosY + height - 1)
            return false;

        return true;
    }

    public static int[,] GetMap(MapLayer layer)
    {
        return (layer == MapLayer.FRONT) ? map[FRONT_MAP] : map[BACK_MAP];
    }

    /// <summary>
    /// Return submap array from map. If it impossible - return null.
    /// </summary>
    /// <param name="startPosX"></param>
    /// <param name="startPosY"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    public static int[,] GetSubMap(int startPosX, int startPosY, int width, int height, MapLayer layer)
    {
        if(CheckSubmapAvailability(startPosX, startPosY, width, height, layer))
        {
            int[,] map = GetMap(layer);
            int[,] data = new int[width, height];
            for (int i = 0, x = startPosX; x < startPosX + width; x++, i++)
            {
                for (int j = 0, y = startPosY; y < startPosY + height; y++, j++)
                {
                    data[i, j] = map[x, y];
                }
            }
            return data;
        }
        return null;
    }      
    
    /// <summary>
    /// Insert submap into map array. true - if possible, if not - false.
    /// </summary>
    /// <param name="startPosX"></param>
    /// <param name="startPosY"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public static bool SetSubMap(int startPosX, int startPosY, int[,] data, MapLayer layer)
    {
        if(CheckSubmapAvailability(startPosX, startPosY, data.GetUpperBound(0), data.GetUpperBound(1), layer))
        {
            int[,] map = GetMap(layer);
            for (int i = 0, x = startPosX; x < data.GetUpperBound(0); x++, i++)
            {
                for (int j = 0, y = startPosY; y < data.GetUpperBound(1); y++, j++)
                {
                    map[x, y] = data[i, j];
                }
            }
            return true;
        }
        return false;
    }

}
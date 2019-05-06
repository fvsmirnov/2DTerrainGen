using GroundTile;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : Singleton<TerrainManager>
{
    public Vector2Int terrainSize = new Vector2Int();
    public int octaves = 5;
    public int offset = 0;
    public int seed = -1;
    [Range(0, 20)] public float frequency = 2f;  //Default value - 2f
    [Range(0, 1.7f)] public float amplitude = 1f;   //Default value - 1f
    [Space]
    public GroundTileData terrainTiles;
    public OreTileData oreTiles;

    private ChunkManager chunkManager;
    private Noise noise;
    private int[] heights;

    private void Awake()
    {
        chunkManager = GetComponent<ChunkManager>();
        noise = new Noise(seed);
    }

    void Start()
    {
        GenerateTerrain();
    }

    #region Generate Terain
    public void GenerateTerrain()
    {
        GenerateMapData();
        GenerateChunks();
    }

    /// <summary>
    /// Generate 2D array of ground data
    /// </summary>
    private void GenerateMapData()
    {
        int groundArraySize = terrainTiles.tiles.Length;
        int groundLayers = Mathf.FloorToInt(terrainSize.y / groundArraySize);

        GenerateHeightMap();

        //Initialize first layer map data
        Map.map[0] = new int[terrainSize.x, terrainSize.y];
        CalculateTileIdMap();

        //Clone data to second array
        Map.map[1] = Map.map[0].Clone() as int[,]; //As we generate terrain first time value for both layers will be identical.
    }

    private void GenerateHeightMap()
    {
        heights = new int[terrainSize.x];
        for (int i = 0; i < terrainSize.x; i++)
        {
            //Generate height point via noise
            int hPoint = Mathf.FloorToInt(noise.PerlinNoise1D((float)i / terrainSize.x, octaves, frequency, amplitude, offset) * terrainSize.y);    //Get height value
            heights[i] = Mathf.Clamp(hPoint, 0, terrainSize.y);
        }
    }

    private void CalculateTileIdMap()
    {
        var tTiles = terrainTiles.tiles;
        var oTiles = oreTiles.ores;

        for (int x = 0; x < terrainSize.x; x++)
        {
            for (int y = 0; y < terrainSize.y; y++)
            {
                //Place ground tile
                for (int i = 0; i < tTiles.Length; i++)
                {
                    if (CheckTilePlace(x, y, terrainTiles.tiles[i].offset, terrainTiles.tiles[i].height, 0, 0, 0))
                    {
                        Map.map[0][x, y] = terrainTiles.tiles[i].tileId;
                        break;
                    }     
                }

                //Place ore tile
                for (int i = 0; i < oTiles.Length; i++)
                {
                    if (CheckTilePlace(x, y, 0, terrainSize.y, oTiles[i].firstNoiseFriction, oTiles[i].secondNoiseFriction, 0.62f))
                    {
                        Map.map[0][x, y] = oreTiles.ores[i].tileId;
                        break;
                    }
                }
            }
        }

    }

    /// <summary>
    /// Check tile place probability via noise
    /// </summary>
    private bool CheckTilePlace(int x, int y, int offset, int height, float firstNoiseFriction = 1, float secondNoiseFriction = 1, float passValue = 0.7f)
    {
        if (y > heights[x])
            return false;

        if(y <= heights[x] - offset && y >= heights[x] - height - offset)
        {
            if (passValue == 0)
                return true;
            else
            {
                return ((Mathf.PerlinNoise(x * firstNoiseFriction, y * firstNoiseFriction) 
                            + Mathf.PerlinNoise(x * secondNoiseFriction, y * secondNoiseFriction)) / 2) >= passValue;
            }    
        }
        return false;
    }

    private void GenerateChunks()
    {
        int xChunkCount = Mathf.FloorToInt(terrainSize.x / ChunkManager.chunkSize.x);
        int yChunkCount = Mathf.FloorToInt(terrainSize.y / ChunkManager.chunkSize.y);
        for (int x = 0; x < xChunkCount; x++)
        {
            for (int y = 0; y < yChunkCount; y++)
            {
                chunkManager.CreateChunk(x, y);
            }
        }
    }
    #endregion
}

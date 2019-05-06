using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class Chunk : MonoBehaviour
{
    //Chunk Data
    public GameObject frontLayer;
    public GameObject backLayer;
    [SerializeField] private Vector3Int startPosition = Vector3Int.zero;
    [SerializeField] private Vector3Int chunkSize = Vector3Int.zero;
    [NonSerialized] public Bounds bounds;
    [NonSerialized] private BoxCollider2D mainCollider;

    //Chunk child data
    [SerializeField] private int[,] frontTilesData;
    [SerializeField] private int[,] backTilesData;
    [NonSerialized] private Tilemap[] tilemaps;
    [NonSerialized] private TilemapRenderer[] tRenderers;

    public bool LoadState { get; private set; } = true;

    //Init properties
    private void Awake()
    {
        tilemaps = new Tilemap[2];
        tRenderers = new TilemapRenderer[2];

        //Front layer init
        tilemaps[0] = frontLayer.GetComponent<Tilemap>();
        tRenderers[0] = frontLayer.GetComponent<TilemapRenderer>();

        //Back layer init
        tilemaps[1] = backLayer.GetComponent<Tilemap>();
        tRenderers[1] = backLayer.GetComponent<TilemapRenderer>();

        mainCollider = GetComponent<BoxCollider2D>();
        
    }

    private void Start()
    {
        mainCollider.offset = new Vector2(chunkSize.x / 2, chunkSize.y / 2);
        mainCollider.size = new Vector2(chunkSize.x, chunkSize.y);
    }

    /// <summary>
    /// Initialize chunk data
    /// </summary>
    /// <param name="startPos">Chunk position</param>
    /// <param name="chunkSize">Chunk size</param>
    public void Initialize(Vector3Int startPos, Vector3Int chunkSize)
    {
        //Set chunk data
        transform.position = startPosition = startPos;
        this.chunkSize = chunkSize;

        //Set chunk bound data
        bounds = new Bounds(new Vector3(
            startPos.x + chunkSize.x / 2f,
            startPos.y + chunkSize.y / 2f,
            0), chunkSize);

        //Set all Tilemap renderer same chunk size
        foreach (var tRenderer in tRenderers)
            tRenderer.chunkSize = chunkSize;

        //Fill front tilemap
        frontTilesData = Map.GetSubMap(startPos.x, startPos.y, chunkSize.x, chunkSize.y, MapLayer.FRONT);
        FillChunk(frontTilesData, MapLayer.FRONT);

        //Fill back tilemap
        backTilesData = Map.GetSubMap(startPos.x, startPos.y, chunkSize.x, chunkSize.y, MapLayer.BACK);
        FillChunk(backTilesData, MapLayer.BACK);

        Load(false);
    }

    private void FillChunk(int[,] data, MapLayer layer)
    {
        if (data != null)
        {
            Vector3Int position;
            for (int x = 0; x < chunkSize.x; x++)
            {
                for (int y = 0; y < chunkSize.y; y++)
                {
                    position = new Vector3Int(x, y, 0);
                    UpdateTile(position, data[position.x, position.y], layer);
                }
            }
        }
    }

    Tile tile = null;
    /// <summary>
    /// Update tile in current position
    /// </summary>
    private void UpdateTile(Vector3Int position, int tileId, MapLayer layer)
    {
        TileManager.tiles.TryGetValue(tileId, out tile);
        if (tile == null)
            return;

        Tilemap map = GetTilemap(layer);
        map.SetTile(position, tile);
    }

    /// <summary>
    /// Load/Unload chunk child data as tilemaps, renderers, colliders etc.
    /// </summary>
    public void Load(bool state)
    {
        if (LoadState != state)
        {
            frontLayer.SetActive(state);
            backLayer.SetActive(state);

            LoadState = state;
        }
    }

    /// <summary>
    /// Set tile on prefered layer
    /// </summary>
    public bool SetTile(Vector3Int tilePos, int id, MapLayer layer)
    {
        //Get tile position in chunk
        Vector3Int position = tilePos - startPosition;
        if (CheckTileExist(tilePos))
        {
            int[,] data = GetTilemapIdData(layer);
            data[position.x, position.y] = id;
            UpdateTile(tilePos, id, layer);
            return true;
        }
        return false;
    }

    public bool RemoveTile(Vector3Int tilePos, MapLayer layer)
    {
        //Get tile position in chunk
        Vector3Int position = tilePos - startPosition;
        if (CheckTileExist(position))
        {
            int[,] data = GetTilemapIdData(layer);
            data[position.x, position.y] = 0;
            UpdateTile(position, 0, layer);
            return true;
        }
        return false;
    }

    bool CheckTileExist(Vector3Int tilePos)
    {
        if (tilePos.x >= 0 && tilePos.x < chunkSize.x
            && tilePos.y >= 0 && tilePos.y < chunkSize.y)
            return true;

        return false;
    }

    Tilemap GetTilemap(MapLayer layer)
    {
        return (layer == MapLayer.FRONT) ? tilemaps[0] : tilemaps[1];
    }

    int[,] GetTilemapIdData(MapLayer layer)
    {
        return (layer == MapLayer.FRONT) ? frontTilesData : backTilesData;
    }

    /// <summary>
    /// Return smallest distance between the point and chunk bounding box
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    public float GetNearestDstToBound(Vector3 point)
    {
        return Mathf.Sqrt(bounds.SqrDistance(point));
    }
}

public enum MapLayer
{
    FRONT,
    BACK
}
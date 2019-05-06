using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkLoadController : MonoBehaviour
{
    public Transform viewer;
    private int chunksVisible = 1;
    private List<Chunk> loadedChunks = new List<Chunk>();
    
    void Update()
    {
        UpdateVisibleChunks();
    }

    int currentChunkCoordX;
    int currentChunkCoordY;
    void UpdateVisibleChunks()
    {
        DisbleLoadedChunks();

        currentChunkCoordX = Mathf.RoundToInt(viewer.position.x / ChunkManager.chunkSize.x);
        currentChunkCoordY = Mathf.RoundToInt(viewer.position.y / ChunkManager.chunkSize.y);

        for (int yOffset = -chunksVisible; yOffset <= chunksVisible; yOffset++)
        {
            for (int xOffset = -chunksVisible; xOffset <= chunksVisible; xOffset++)
            {
                Vector3Int viewerChunkCoord = new Vector3Int((currentChunkCoordX + xOffset) * ChunkManager.chunkSize.x,
                                                             (currentChunkCoordY + yOffset) * ChunkManager.chunkSize.y, 0);

                if (ChunkManager.chunkArray.ContainsKey(viewerChunkCoord))
                {
                    ChunkManager.chunkArray[viewerChunkCoord].Load(true);
                    loadedChunks.Add(ChunkManager.chunkArray[viewerChunkCoord]);
                }
                    
            }
        }

    }

    void DisbleLoadedChunks()
    {
        for (int i = 0; i < loadedChunks.Count; i++)
        {
            loadedChunks[i].Load(false);
        }
        loadedChunks.Clear();
    }

}

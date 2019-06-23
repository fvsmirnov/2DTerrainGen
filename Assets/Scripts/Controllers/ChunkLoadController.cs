using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrainGen
{
    public class ChunkLoadController : MonoBehaviour
    {
        public Transform viewer;
        public Vector2Int chunkVisibleRadius = Vector2Int.zero;

        private int currentViewerCoordX, currentViewerCoordY;
        private Vector3 prevViewerPos = Vector3.zero;
        private List<Chunk> loadedChunks = new List<Chunk>();

        void Update()
        {
            CheckVisibleChunks();
            //UpdateVisibleChunks();
        }

        void CheckVisibleChunks()
        {
            //Update viewer coords
            currentViewerCoordX = Mathf.FloorToInt(viewer.position.x / ChunkManager.chunkSize.x);
            currentViewerCoordY = Mathf.FloorToInt(viewer.position.y / ChunkManager.chunkSize.y);

            if (prevViewerPos.x != currentViewerCoordX || prevViewerPos.y != currentViewerCoordY)
            {
                prevViewerPos = new Vector3(currentViewerCoordX, currentViewerCoordY, 0);
                UpdateVisibleChunks();
            }
        }

        void UpdateVisibleChunks()
        {
            DisbleLoadedChunks();

            for (int yOffset = -chunkVisibleRadius.y; yOffset <= chunkVisibleRadius.y; yOffset++)
            {
                for (int xOffset = -chunkVisibleRadius.x; xOffset <= chunkVisibleRadius.x; xOffset++)
                {
                    Vector3Int viewerChunkCoord = new Vector3Int((currentViewerCoordX + xOffset) * ChunkManager.chunkSize.x,
                                                                 (currentViewerCoordY + yOffset) * ChunkManager.chunkSize.y, 0);

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
}

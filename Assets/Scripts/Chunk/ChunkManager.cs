using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    public static Dictionary<Vector3Int, Chunk> chunkArray = new Dictionary<Vector3Int, Chunk>();
    public static Vector3Int chunkSize = new Vector3Int(32, 32, 0);
    public GameObject chunkPrefab;

    private List<GameObject> activeChunkList = new List<GameObject>();

    public void CreateChunk(int x, int y)
    {
        int xPos = x * chunkSize.x;
        int yPos = y * chunkSize.y;

        if (!CheckChunkAt(xPos, yPos))
        {
            GameObject go = Instantiate(chunkPrefab, transform);
            Vector3Int position = new Vector3Int(xPos, yPos, 0);

            Chunk chunk = go.GetComponent<Chunk>();
            chunk.Initialize(position, chunkSize);
            AddChunkToArray(position, chunk);
        }
    }

    RaycastHit2D hit;
    /// <summary>
    /// Check if chunk already created in current position
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public bool CheckChunkAt(int x, int y)
    {
        hit = Physics2D.Raycast(new Vector2(x, y), Vector2.zero);
        if (hit.collider != null && hit.collider.tag == "Chunk")
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Return chunk if it exist in current position. If not - return null.
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public Chunk GetChunkAt(Vector3Int pos)
    {
        if (CheckChunkAt(pos.x, pos.y))
            return hit.collider.GetComponent<Chunk>();

        return null;
    }

    void AddChunkToArray(Vector3Int chunkPosition, Chunk chunk)
    {
        if(!chunkArray.ContainsKey(chunkPosition))
            chunkArray.Add(chunkPosition, chunk);
    }
}

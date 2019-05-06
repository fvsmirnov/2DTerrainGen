using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigController : MonoBehaviour
{
    Grid grid;

    private void Start()
    {
        grid = FindObjectOfType<Grid>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            Dig(MapLayer.FRONT);

        if (Input.GetMouseButtonDown(1))
            Dig(MapLayer.BACK);
    }

    void Dig(MapLayer layer)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);


        if (hit.collider != null)
        {
            Chunk chunk = hit.collider.GetComponentInParent<Chunk>();
            if(chunk != null)
            {
                Vector3Int digPosition = grid.WorldToCell(hit.point);
                chunk.RemoveTile(digPosition, layer);
            }
        }
    }
}

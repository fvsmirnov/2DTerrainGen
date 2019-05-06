using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GroundTile
{
    [CreateAssetMenu(fileName = "GroundTileData", menuName = "GroundTileData")]
    public class GroundTileData : ScriptableObject
    {
        //Ground tiles
        public GroundData[] tiles;
    }

    [Serializable]
    public struct GroundData
    {
        public int tileId;
        public int offset;
        public int height;
    }
}

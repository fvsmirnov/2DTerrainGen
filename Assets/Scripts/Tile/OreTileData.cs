using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GroundTile
{
    [CreateAssetMenu(fileName = "OreTileData", menuName = "OreTileData")]
    public class OreTileData : ScriptableObject
    {
        public OreData[] ores;
    }

    [Serializable]
    public struct OreData
    {
        public int tileId;
        public float firstNoiseFriction;
        public float secondNoiseFriction;
    }
}

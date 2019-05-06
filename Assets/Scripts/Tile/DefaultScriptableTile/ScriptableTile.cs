using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// This class created cause default Tile asset don't provide "Instance GameObject" field
/// </summary>
[CreateAssetMenu(menuName = "Tiles/Scriptable Tile", fileName = "ScriptableTile")]
public class ScriptableTile : Tile{ }

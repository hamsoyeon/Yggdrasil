using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TilePrefab",menuName = "Tile/Prefab")]
public class TilePrefab : ScriptableObject
{
    public GameObject TileObj;
    public bool isWall;
    public bool isBuffTile;
}

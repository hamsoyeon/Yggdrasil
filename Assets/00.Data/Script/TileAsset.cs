using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TileAssets", menuName = "Tile/StageTile")]
public class TileAsset : ScriptableObject
{
    [SerializeField]
    public List<TilePrefab> m_prefab;
}

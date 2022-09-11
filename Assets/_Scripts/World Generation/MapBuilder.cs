using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace WorldGeneration {
  public class MapBuilder : MonoBehaviour {

    public Tilemap walkableTilemap;
    public Tilemap obstacleTilemap;
    public Tilemap buildingTilemap;

    public TileBase cityTile;

    public void Build(int[] tileIndexMap, int width, int height, TerrainType[] regions, List<int> cityIndices) {

      var positions = new Vector3Int[width * height];
      var walkableTiles = new TileBase[positions.Length];
      var obstacleTiles = new TileBase[positions.Length];

      for (int i = 0; i < positions.Length; i++) {
        positions[i] = new(i % width, i / height);

        TerrainType terrainType = regions[tileIndexMap[i]];

        if (terrainType.isWalkable) {
          walkableTiles[i] = terrainType.tile;
          obstacleTiles[i] = null;

          if (cityIndices.Contains(i)) {
            buildingTilemap.SetTile(positions[i], cityTile);
          }
        } else {
          walkableTiles[i] = null;
          obstacleTiles[i] = terrainType.tile;
        }
      }

      walkableTilemap.SetTiles(positions, walkableTiles);
      obstacleTilemap.SetTiles(positions, obstacleTiles);
    }
  }
}
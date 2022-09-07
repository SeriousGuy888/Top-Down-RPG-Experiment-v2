using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace WorldGeneration {
  public class MapBuilder : MonoBehaviour {

    public Tilemap outputTilemap;

    public void Build(int[] tileIndexMap, int width, int height, TerrainType[] regions) {

      var positions = new Vector3Int[width * height];
      var tileArray = new TileBase[positions.Length];

      for (int i = 0; i < positions.Length; i++) {
        positions[i] = new(i % width, i / height);
        tileArray[i] = regions[tileIndexMap[i]].tile;
      }

      outputTilemap.SetTiles(positions, tileArray);
    }
  }
}
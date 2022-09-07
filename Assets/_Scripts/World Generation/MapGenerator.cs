using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace WorldGeneration {
  public class MapGenerator : MonoBehaviour {

    public MapBuilder builder;

    public int width;
    public int height;
    public float noiseScale;

    public int octaves;
    [Range(0, 1)] public float persistance;
    public float lacunarity;

    public int seed;
    public Vector2 offset;

    public TerrainType[] regions;


    public void GenerateMap() {
      var noiseMap = Noise.GenerateNoiseMap(width, height, seed, noiseScale, octaves, persistance, lacunarity, offset);

      var tileIndexMap = new int[width * height];

      for (int y = 0; y < height; y++) {
        for (int x = 0; x < width; x++) {
          float currHeight = noiseMap[x, y];

          for (int i = 0; i < regions.Length; i++) {
            if (currHeight <= regions[i].height) {
              tileIndexMap[y * width + x] = i;
              break;
            }
          }
        }
      }

      builder.Build(tileIndexMap, width, height, regions);
    }

    private void OnValidate() {
      if (width < 1)
        width = 1;
      if (height < 1)
        height = 1;
      if (lacunarity < 1)
        lacunarity = 1;
      if (octaves < 0)
        octaves = 0;
    }
  }

  [System.Serializable]
  public struct TerrainType {
    public float height;
    public TileBase tile;
  }
}
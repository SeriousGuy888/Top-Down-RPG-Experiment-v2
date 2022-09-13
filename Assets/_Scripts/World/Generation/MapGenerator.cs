using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace WorldGeneration {
  public class MapGenerator : MonoBehaviour {

    public MapBuilder builder;

    [Header("Terrain Noise Settings")]
    public int width;
    public int height;
    public float noiseScale;
    public int seed;
    public Vector2 offset;

    public int octaves;
    [Range(0, 1)] public float persistance;
    public float lacunarity;

    public TerrainType[] regions;

    [Header("City Placement Settings")]
    public int cityZoneDim;


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

      // Cut up the world into square zones and choose a random position in each zone to spawn a city.
      List<int> citySpawnIndices = new();
      int cityZoneCountX = Mathf.CeilToInt((float)width / cityZoneDim);
      int cityZoneCountY = Mathf.CeilToInt((float)height / cityZoneDim);
      int citySpawnAttempts = cityZoneCountX * cityZoneCountY;
      Debug.Log(citySpawnAttempts);

      UnityEngine.Random.InitState(seed);
      for (int zoneY = 0; zoneY < cityZoneCountY; zoneY++) {
        for (int zoneX = 0; zoneX < cityZoneCountX; zoneX++) {
          int minX = zoneX * cityZoneDim;
          int maxX = minX + cityZoneDim;
          int minY = zoneY * cityZoneDim;
          int maxY = minY + cityZoneDim;

          int x = UnityEngine.Random.Range(minX, maxX);
          int y = UnityEngine.Random.Range(minY, maxY);

          citySpawnIndices.Add(y * width + x);
        }
      }

      builder.Build(tileIndexMap, width, height, regions, citySpawnIndices);
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
    public bool isWalkable;
    public TileBase tile;
  }
}
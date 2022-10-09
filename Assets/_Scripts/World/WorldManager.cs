using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour {
  public static WorldManager Instance;

  public Grid grid;
  public GameObject cityPrefab;

  private List<City> cities;

  private void Awake() {
    Instance = this;
    cities = new();
  }

  public void SpawnCity(Vector2Int pos) {
    City city = Instantiate(cityPrefab, grid.GetCellCenterWorld((Vector3Int)pos), Quaternion.identity).GetComponent<City>();
    city.Init(pos);

    cities.Add(city);
  }
}

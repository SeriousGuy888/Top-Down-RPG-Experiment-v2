using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
  public GameObject prefab;
  public float spawnInterval;

  private void Start() {
    StartCoroutine(CheckSpawning());
  }

  public IEnumerator CheckSpawning() {
    while(true) {
      float playerDist = (GameManager.Instance.player.transform.position - transform.position).magnitude;
      if(playerDist <= 20)
        Instantiate(prefab, transform.position, Quaternion.identity);
      yield return new WaitForSeconds(spawnInterval);
    }
  }
}

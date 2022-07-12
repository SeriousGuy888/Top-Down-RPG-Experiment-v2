using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
  public int space = 12;
  public List<Item> items = new();

  public bool Add(Item item) {
    if (items.Count >= space) {
      Debug.Log("not enough room");
      return false;
    }
    items.Add(item);
    return true;
  }

  public void Remove(Item item) {
    items.Remove(item);
  }
}

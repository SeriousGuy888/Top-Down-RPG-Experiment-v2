using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
  public static Inventory Instance;
  private void Awake() {
    Instance = this;
  }

  public delegate void OnItemChanged();
  public OnItemChanged onItemChangedCallback;

  public int space = 12;
  public List<Item> items = new();

  public bool Add(Item item) {
    if (items.Count >= space) {
      Debug.Log("not enough room");
      return false;
    }

    items.Add(item);
    if (onItemChangedCallback != null)
      onItemChangedCallback.Invoke();
    return true;
  }

  public void Remove(Item item) {
    items.Remove(item);
    if (onItemChangedCallback != null)
      onItemChangedCallback.Invoke();
  }
}

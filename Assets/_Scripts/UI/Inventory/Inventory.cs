using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
  public static Inventory Instance;
  private void Awake() {
    Instance = this;
  }

  private InputMaster controls;

  public delegate void OnItemChanged();
  public OnItemChanged onItemChangedCallback;

  public InventoryUI inventoryUI;
  public int inventorySize = 12;
  public List<Item> items = new();
  

  private void Start() {
    inventoryUI.InitInventoryUI(inventorySize);

    controls = GameManager.Instance.controls;
    controls.Player.ToggleInventory.performed += _ => {
      if (inventoryUI.gameObject.activeSelf) {
        inventoryUI.Hide();
      } else {
        inventoryUI.Show();
      }
    };
  }

  public bool Add(Item item) {
    if (items.Count >= inventorySize) {
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

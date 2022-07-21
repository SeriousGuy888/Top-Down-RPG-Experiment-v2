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
  public Item[] items;
  

  private void Start() {
    items = new Item[inventorySize];
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
    for (int i = 0; i < items.Length; i++) {
      if(items[i] == null) {
        items[i] = item;

        if (onItemChangedCallback != null)
          onItemChangedCallback.Invoke();
        return true;
      }
      continue;
    }

    Debug.Log("not enough room");
    return false;
  }

  public void Remove(int slotIndex) {
    items[slotIndex] = null;

    if (onItemChangedCallback != null)
      onItemChangedCallback.Invoke();
  }
}

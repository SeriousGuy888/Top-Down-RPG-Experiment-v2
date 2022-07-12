using System.Net.Sockets;
using UnityEngine;

public class InventoryUI : MonoBehaviour {
  public GameObject inventoryUi;
  public Transform itemsParent;
  public GameObject slotPrefab;

  private Inventory inventory;
  private InventorySlot[] slots;

  private InputMaster controls;



  private void Start() {
    controls = GameManager.Instance.controls;
    controls.Player.ToggleInventory.performed += _ => ToggleInventory();

    inventory = GameManager.Instance.player.inventory;
    inventory.onItemChangedCallback += UpdateUI;

    foreach (Transform child in itemsParent.transform)
      child.gameObject.SetActive(false);
    for (int i = 0; i < inventory.space; i++)
      Instantiate(slotPrefab, itemsParent);
    slots = itemsParent.GetComponentsInChildren<InventorySlot>();
  }

  private void ToggleInventory() {
    inventoryUi.SetActive(!inventoryUi.activeSelf);
  }

  private void UpdateUI() {
    for (int i = 0; i < slots.Length; i++) {
      if(!slots[i].gameObject.activeSelf) {
        i--;
        continue;
      }

      if (i < inventory.items.Count)
        slots[i].SetItem(inventory.items[i]);
      else
        slots[i].ClearSlot();
    }
  }
}

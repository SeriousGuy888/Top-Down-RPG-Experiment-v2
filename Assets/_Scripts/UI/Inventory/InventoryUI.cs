using System.Net.Sockets;
using UnityEditor;
using UnityEngine;

public class InventoryUI : MonoBehaviour {
  public Inventory inventory;
  public Equipment equipment;

  public InventorySlot slotPrefab;

  public Transform inventorySlotsContainer;
  private InventorySlot[] inventorySlots;


  private void Start() {
    Hide();

    inventory.onItemChangedCallback += UpdateUI;
    equipment.onItemChangedCallback += UpdateUI;
  }

  public void InitInventoryUI(int inventorySize) {
    inventorySlots = new InventorySlot[inventorySize];
    for (int i = 0; i < inventorySize; i++) {
      var newSlot = Instantiate(slotPrefab, Vector3.zero, Quaternion.identity);
      newSlot.transform.SetParent(inventorySlotsContainer);
      inventorySlots[i] = newSlot;
    }
  }

  public void Show() => gameObject.SetActive(true);
  public void Hide() => gameObject.SetActive(false);

  private void UpdateUI() {
    for (int i = 0; i < inventorySlots.Length; i++) {
      if (i < inventory.items.Count)
        inventorySlots[i].SetItem(inventory.items[i]);
      else
        inventorySlots[i].ClearSlot();
    }

    // for (int i = 0; i < equipSlots.Length; i++) {
    //   Item item = equipment.items[i];
    //   if(item != null)
    //     equipSlots[i].SetItem(item);
    //   else
    //     equipSlots[i].ClearSlot();
    // }
  }
}

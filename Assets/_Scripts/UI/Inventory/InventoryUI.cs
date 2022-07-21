using System.Net.Sockets;
using UnityEditor;
using UnityEngine;

public class InventoryUI : MonoBehaviour {
  public Inventory inventory;
  public Equipment equipment;

  public InventorySlot slotPrefab;

  public Transform inventorySlotsContainer;
  public InventoryDescription descriptionPanel;

  private InventorySlot[] inventorySlots;

  private void Awake() {
    Hide();

    descriptionPanel.ResetDescription();

    // inventory.onItemChangedCallback += UpdateUI;
    // equipment.onItemChangedCallback += UpdateUI;
  }

  public void Show() {
    gameObject.SetActive(true);
    descriptionPanel.ResetDescription();

    for(int i = 0; i < inventory.items.Count; i++) {
      Item item = inventory.items[i];
      inventorySlots[i].SetData(item.icon, 1);
    }
  }
  public void Hide() => gameObject.SetActive(false);

  public void InitInventoryUI(int inventorySize) {
    inventorySlots = new InventorySlot[inventorySize];
    for (int i = 0; i < inventorySize; i++) {
      var newSlot = Instantiate(slotPrefab, Vector3.zero, Quaternion.identity);
      newSlot.transform.SetParent(inventorySlotsContainer);
      inventorySlots[i] = newSlot;

      newSlot.OnItemClicked += HandleItemSelect;
      newSlot.OnItemRightClicked += HandleItemShowActions;
      newSlot.OnItemDragStart += HandleDragStart;
      newSlot.OnItemDragEnd += HandleDragEnd;
      newSlot.OnItemDroppedOn += HandleSwap;
    }
  }

  private void HandleItemSelect(InventorySlot slot) {
    Debug.Log(slot.name);
  }
  private void HandleItemShowActions(InventorySlot slot) { }
  private void HandleDragStart(InventorySlot slot) { }
  private void HandleDragEnd(InventorySlot slot) { }
  private void HandleSwap(InventorySlot slot) { }


  private void UpdateUI() {
    // for (int i = 0; i < inventorySlots.Length; i++) {
    //   if (i < inventory.items.Count)
    //     inventorySlots[i].SetItem(inventory.items[i]);
    //   else
    //     inventorySlots[i].ClearSlot();
    // }

    // for (int i = 0; i < equipSlots.Length; i++) {
    //   Item item = equipment.items[i];
    //   if(item != null)
    //     equipSlots[i].SetItem(item);
    //   else
    //     equipSlots[i].ClearSlot();
    // }
  }
}

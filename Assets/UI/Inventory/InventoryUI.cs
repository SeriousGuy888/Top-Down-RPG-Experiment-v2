using UnityEngine;

public class InventoryUI : MonoBehaviour {
  public Transform itemsParent;
  public GameObject inventoryUi;
  private Inventory inventory;
  private InventorySlot[] slots;

  private InputMaster controls;



  private void Start() {
    controls = GameManager.Instance.controls;
    controls.Player.ToggleInventory.performed += _ => ToggleInventory();

    inventory = GameManager.Instance.player.inventory;
    inventory.onItemChangedCallback += UpdateUI;

    slots = itemsParent.GetComponentsInChildren<InventorySlot>();
  }

  private void ToggleInventory() {
    inventoryUi.SetActive(!inventoryUi.activeSelf);
  }

  private void UpdateUI() {
    for (int i = 0; i < slots.Length; i++) {
      if(i < inventory.items.Count)
        slots[i].SetItem(inventory.items[i]);
      else
        slots[i].ClearSlot();
    }
  }
}

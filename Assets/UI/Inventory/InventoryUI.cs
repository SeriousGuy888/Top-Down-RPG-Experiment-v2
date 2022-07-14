using System.Net.Sockets;
using UnityEngine;

[ExecuteInEditMode]
public class InventoryUI : MonoBehaviour {
  public Inventory inventory;
  public GameObject inventoryUi;
  public Transform itemsParent;
  public GameObject slotPrefab;

  private InventorySlot[] slots;

  private InputMaster controls;

  private void Start() {
    if(!Application.isPlaying)
      return;

    controls = GameManager.Instance.controls;
    controls.Player.ToggleInventory.performed += _ => ToggleInventory();
    inventoryUi.SetActive(false);

    inventory.onItemChangedCallback += UpdateUI;

    slots = itemsParent.GetComponentsInChildren<InventorySlot>();
  }

  private void Update() {
    if(Application.isPlaying)
      return;

    int existingSlots = itemsParent.childCount;
    int neededSlots = Mathf.Max(inventory.space, 0);


    if (existingSlots < neededSlots) {
      for (int i = 0; i < neededSlots - existingSlots; i++) {
        Instantiate(slotPrefab, itemsParent);
      }
      return;
    }
    while (existingSlots > neededSlots) {
      DestroyImmediate(itemsParent.GetChild(neededSlots).gameObject);
      existingSlots--;
    }
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

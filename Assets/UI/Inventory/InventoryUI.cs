using System.Net.Sockets;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class InventoryUI : MonoBehaviour {
  public Inventory inventory;
  public Transform slotsContainer;
  public GameObject slotPrefab;

  private InventorySlot[] slots;

  private InputMaster controls;

  private void Start() {
    if(!Application.isPlaying)
      return;

    controls = GameManager.Instance.controls;
    controls.Player.ToggleInventory.performed += _ => ToggleInventory();
    slotsContainer.gameObject.SetActive(false);

    inventory.onItemChangedCallback += UpdateUI;

    slots = slotsContainer.GetComponentsInChildren<InventorySlot>();
  }

  private void Update() {
    if(Application.isPlaying)
      return;

    int existingSlots = slotsContainer.childCount;
    int neededSlots = Mathf.Max(inventory.space, 0);


    if (existingSlots < neededSlots) {
      for (int i = 0; i < neededSlots - existingSlots; i++) {
        PrefabUtility.InstantiatePrefab(slotPrefab, slotsContainer);
      }
      return;
    }
    while (existingSlots > neededSlots) {
      DestroyImmediate(slotsContainer.GetChild(neededSlots).gameObject);
      existingSlots--;
    }
  }

  private void ToggleInventory() {
    slotsContainer.gameObject.SetActive(!slotsContainer.gameObject.activeSelf);
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

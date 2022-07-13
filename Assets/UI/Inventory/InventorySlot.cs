using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour {
  public Image icon;

  private Item item;
  
  public void SetItem(Item newItem) {
    item = newItem;
    icon.sprite = item.icon;
    icon.enabled = true;
  }

  public void ClearSlot() {
    item = null;
    icon.sprite = null;
    icon.enabled = false;
  }

  public void UseItem() {
    if(item == null)
      return;

    item.Use();
  }
}
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class InventorySlot : MonoBehaviour {
  public Image icon; // The icon being displayed right now
  public Sprite defaultSprite; // Sprite to display as icon when slot is empty

  private Item item;

  private void Update() {
    if(Application.isPlaying)
      return;
    
    if(item == null)
      SetEmptyIcon();
  }

  public void SetItem(Item newItem) {
    item = newItem;

    icon.sprite = item.icon;
    icon.color = new Color(1, 1, 1, 1);
    icon.enabled = true;
  }

  public void ClearSlot() {
    item = null;
    SetEmptyIcon();
  }

  private void SetEmptyIcon() {
    if (defaultSprite != null) {
      icon.sprite = defaultSprite;
      icon.color = new Color(1, 1, 1, 0.5f);
      icon.enabled = true;
    } else {
      icon.sprite = null;
      icon.enabled = false;
    }
  }

  public void UseItem() {
    if (item == null)
      return;

    item.Use();
  }
}

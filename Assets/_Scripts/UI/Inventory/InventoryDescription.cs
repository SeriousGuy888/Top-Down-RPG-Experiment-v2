using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryDescription : MonoBehaviour {
  public Image itemImage;
  public TMP_Text title;
  public TMP_Text description;

  private void Awake() {
    ResetDescription();
  }

  public void ResetDescription() {
    itemImage.gameObject.SetActive(false);
    title.text = "";
    description.text = "";
  }

  public void SetDescription(Sprite sprite, string titleText, string descriptionText) {
    itemImage.gameObject.SetActive(true);
    itemImage.sprite = sprite;
    title.text = titleText;
    description.text = descriptionText;
  }
}

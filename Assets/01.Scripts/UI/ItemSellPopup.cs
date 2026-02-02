using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSellPopup : MonoBehaviour
{
    [Header("UI")]
    public GameObject itemSellPopup;
    public Image Icon;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI rarityText;
    public TextMeshProUGUI priceText;

    private InventorySlot savedSlot;
    private InventoryItem savedItem;

    public void OpenPopup(InventorySlot slot, InventoryItem item)
    {
        savedSlot = slot;
        savedItem = item;

        Icon.sprite = item.data.icon;
        nameText.text = item.data.itemName;
        rarityText.text = $"{item.rarity}";
        priceText.text = $"{item.GetSellPrice()}";

        itemSellPopup.SetActive(true);
    }

    public void OnClickSellBtn()
    {
        InventoryManager.Instance.SellItem(savedSlot,savedItem);

        OnClickCancel();
    }

    public void OnClickCancel()
    {
        itemSellPopup.SetActive(false);
    }
}

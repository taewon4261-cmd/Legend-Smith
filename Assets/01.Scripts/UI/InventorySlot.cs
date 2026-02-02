using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [Header("UI")]
    public Image iconImage;
    public Image borderImage;

    private InventoryItem itemInfo;

    public void SetSlot(InventoryItem item)
    {
        itemInfo = item;

        iconImage.sprite = item.data.icon;

        if (item.rarity == ItemRarity.Legend) borderImage.color = Color.yellow;
        else if (item.rarity == ItemRarity.Epic) borderImage.color = Color.magenta;
        else if (item.rarity == ItemRarity.Rare) borderImage.color = Color.blue;
        else borderImage.color = Color.white;
    }

    public void OnClickSellButton()
    {
        InventoryManager.Instance.ShowSellPopup(this, itemInfo);
    }

}

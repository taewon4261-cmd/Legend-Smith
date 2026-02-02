using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [Header("UI")]
    public Image iconImage;
    public Image borderImage;

    public void SetSlot(ItemDataSO data, ItemRarity rarity)
    {
        iconImage.sprite = data.icon;

        if (rarity == ItemRarity.Legend) borderImage.color = Color.yellow;
        else if (rarity == ItemRarity.Epic) borderImage.color = Color.magenta;
        else if (rarity == ItemRarity.Rare) borderImage.color = Color.blue;
        else borderImage.color = Color.white;
    }

}

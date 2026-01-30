using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class InventoryItem
{
    public ItemDataSO data;
    public ItemRarity rarity;

    public int GetSellPrice()
    {
        float multiplier = 1f;
        if (rarity == ItemRarity.Rare) multiplier = 1.5f;
        if (rarity == ItemRarity.Epic) multiplier = 2f;
        if (rarity == ItemRarity.Legend) multiplier = 5f;
      
        return Mathf.RoundToInt(data.price * multiplier);
    }

   
}

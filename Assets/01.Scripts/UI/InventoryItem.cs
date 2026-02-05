using UnityEngine;


[System.Serializable]
public class InventoryItem
{
    public ItemDataSO data;
    public ItemRarity rarity;

    public InventoryItem(ItemDataSO data, ItemRarity rarity)
    {
        this.data = data;
        this.rarity = rarity;
    }

    public int GetSellPrice()
    {
        float multiplier = 1f;
        if (rarity == ItemRarity.Rare) multiplier = 1.5f;
        if (rarity == ItemRarity.Epic) multiplier = 2f;
        if (rarity == ItemRarity.Legend) multiplier = 5f;
      
        // 반올림 함수
        return Mathf.RoundToInt(data.price * multiplier);
    }

   
}

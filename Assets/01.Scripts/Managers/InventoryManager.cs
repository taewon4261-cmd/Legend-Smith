using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public List<InventoryItem> myInven = new List<InventoryItem>();

    private void Awake()
    {
        Instance = this;
    }

    public void AddItem(ItemDataSO data, ItemRarity rarity)
    {
        InventoryItem item = new InventoryItem();

        item.data = data;
        item.rarity = rarity;

        myInven.Add(item);

        Debug.Log($"{data.itemName} {rarity} È¹µæ");
    }
}

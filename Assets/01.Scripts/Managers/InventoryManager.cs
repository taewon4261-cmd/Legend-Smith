using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("¿Œ∫•≈‰∏Æ ø¨∞·")]
    public GameObject slotPrefab;
    public Transform contentParent;

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

        GameObject slot = Instantiate(slotPrefab, contentParent);

        slot.GetComponent<InventorySlot>().SetSlot(data, rarity);


        Debug.Log($"{data.itemName} {rarity} »πµÊ");
    }
}

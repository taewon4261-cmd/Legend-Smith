using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("인벤토리 연결")]
    public GameObject slotPrefab;
    public Transform contentParent;

    public ItemSellPopup popupScript;

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
        InventorySlot slotScript = slot.GetComponent<InventorySlot>();

        slotScript.SetSlot(item);

        Debug.Log($"{data.itemName} {rarity} 획득");
    }

    public void SellItem(InventorySlot slot, InventoryItem item)
    {
        int price = item.GetSellPrice();

        ResourceManager.Instance.AddGold(price);

        myInven.Remove(item);

        Destroy(slot.gameObject);

        Debug.Log($" {item.data.itemName} 판매 완료! +{price}원");
    }

    public void ShowSellPopup(InventorySlot slot, InventoryItem item)
    {
        popupScript.OpenPopup(slot, item);
    }
}

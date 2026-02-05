using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("인벤토리 연결")]
    public GameObject slotPrefab; // 생성할 슬롯 프리팹
    public Transform contentParent; // 슬롯이 배치될 Grid 부모
    public ItemSellPopup popupScript;

    // 소지중인 아이템 데이터 리스트
    public List<InventoryItem> myInven = new List<InventoryItem>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    /// <summary>
    ///  새로운 아이템을 획득하여 리스트에 추가하고 실제 UI 슬롯을 생성하는 함수
    /// </summary>
    /// <param name="data"> 아이템 정보 SO </param>
    /// <param name="rarity"> 아이템 등급 </param>
    public void AddItem(ItemDataSO data, ItemRarity rarity)
    {
        // 데이터 생성 및 리스트 추가
        InventoryItem item = new InventoryItem(); 
        item.data = data;
        item.rarity = rarity;
        myInven.Add(item);

        // 인벤토리 UI 슬롯 생성 및 데이터 전달
        GameObject slot = Instantiate(slotPrefab, contentParent);
        InventorySlot slotScript = slot.GetComponent<InventorySlot>();
        slotScript.SetSlot(item);
    }

    /// <summary>
    /// 아이템을 판매하여 골드를 획득하고 목록에서 제거하는 함수
    /// </summary>
    /// <param name="slot"> 제거할 UI 슬롯 </param>
    /// <param name="item"> 제거할 아이템 데이터 </param>
    public void SellItem(InventorySlot slot, InventoryItem item)
    {
        // 판매금액 매니저에 반영
        int price = item.GetSellPrice();
        ResourceManager.Instance.AddGold(price);

        // 데이터 및 UI 제거
        myInven.Remove(item);
        Destroy(slot.gameObject);
    }

    // 클릭 시 아이템 판매 버튼 팝업창 표시
    public void ShowSellPopup(InventorySlot slot, InventoryItem item)
    {
        popupScript.OpenPopup(slot, item);
    }
}

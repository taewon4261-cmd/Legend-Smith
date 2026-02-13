using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemSaveInfo
{
    public string itemName;
    public ItemRarity rarity;
}

[System.Serializable]
public class InventorySaveData
{
    public List<ItemSaveInfo> saveItems = new List<ItemSaveInfo>();
}
public class InventoryManager : MonoBehaviour
{
    public WeaponDBSO weaponDB;

    [Header("인벤토리 연결")]
    public GameObject slotPrefab; // 생성할 슬롯 프리팹
    public Transform contentParent; // 슬롯이 배치될 Grid 부모
    public ItemSellPopup popupScript;

    // 소지중인 아이템 데이터 리스트
    public List<InventoryItem> myInven = new List<InventoryItem>();

    private const string invenWeaponKey = "Inven_";

    private Queue<GameObject> slotPool = new Queue<GameObject>();

    public void Init()
    {
        LoadInventory();
    }

    private GameObject GetSlotFromPool()
    {
        GameObject slotObj;
        if (slotPool.Count > 0)
        {
            slotObj = slotPool.Dequeue();
            slotObj.SetActive(true);
        }
        else
        {
            slotObj = Instantiate(slotPrefab, contentParent);
        }

        // 계층 구조상 가장 아래로 보내서 순서 유지
        slotObj.transform.SetAsLastSibling();

        return slotObj;
    }

    void ReturnSlotToPool(GameObject slotObj)
    {
        slotObj.SetActive(false); // 비활성화
        slotPool.Enqueue(slotObj); // 풀에 넣기
    }

    /// <summary>
    ///  새로운 아이템을 획득하여 리스트에 추가하고 실제 UI 슬롯을 생성하는 함수
    /// </summary>
    /// <param name="data"> 아이템 정보 SO </param>
    /// <param name="rarity"> 아이템 등급 </param>
    public void AddItem(ItemDataSO data, ItemRarity rarity)
    {
        // 데이터 생성 및 리스트 추가
        InventoryItem item = new InventoryItem(data,rarity); 
        myInven.Add(item);

        // 인벤토리 UI 슬롯 생성 및 데이터 전달
        GameObject slot = GetSlotFromPool();
        InventorySlot slotScript = slot.GetComponent<InventorySlot>();
        slotScript.SetSlot(item);

        SaveInventory();
    }

    public void SaveInventory()
    {
        InventorySaveData pack = new InventorySaveData();

        foreach (var item in myInven)
        {
            pack.saveItems.Add(new ItemSaveInfo
            {
                itemName = item.data.itemName,
                rarity = item.rarity

            });
        }

        string jsonString = JsonUtility.ToJson(pack);

        PlayerPrefs.SetString(invenWeaponKey, jsonString);
        PlayerPrefs.Save();
    }

    public void LoadInventory()
    {
        if(!PlayerPrefs.HasKey(invenWeaponKey)) return;

        string jsonString = PlayerPrefs.GetString(invenWeaponKey);

        InventorySaveData pack = JsonUtility.FromJson<InventorySaveData>(jsonString);

        myInven.Clear();

        // contentParent.childCount : 유니티 변수 ( 현재 내 자식 오브젝트가 몇 개인지 실시간으로 세어주는 기능)
        int myChildCount = contentParent.childCount;
        for (int i = myChildCount - 1; i >= 0; i--)
        {
            GameObject child = contentParent.GetChild(i).gameObject;

            if (child.activeSelf)
            {
                ReturnSlotToPool(child);
            }
        }

        foreach (var info in pack.saveItems)
        {
            ItemDataSO originalData = weaponDB.allWeapon.Find(X => X.itemName == info.itemName);

            if (originalData != null)
            {
                InventoryItem item = new InventoryItem(originalData, info.rarity);
                myInven.Add(item);

                GameObject slot = GetSlotFromPool();

                InventorySlot slotScript = slot.GetComponent<InventorySlot>();
                slotScript.SetSlot(item);
            }
        }


    }

    /// <summary>
    /// 아이템을 판매하여 골드를 획득하고 목록에서 제거하는 함수
    /// </summary>
    /// <param name="slot"> 제거할 UI 슬롯 </param>
    /// <param name="item"> 제거할 아이템 데이터 </param>
    public void SellItem(InventorySlot slot, InventoryItem item)
    {
        // 판매금액 매니저에 반영
        float bonusRate = GameManager.Instance.Upgrade.GetTotalBonusValue(UpgradeType.SellPrice);
        int OriginalPrice = item.GetSellPrice();
        int finalPrice = Mathf.RoundToInt(OriginalPrice * (1 + bonusRate));

        GameManager.Instance.Resource.AddGold(finalPrice);

        GameManager.Instance.Quest.NotifyQuestAction(QuestType.Sell, 1);

        GameManager.Instance.SFX.PlaySFX("BuySellUpgrade", 1);

        // 데이터 및 UI 제거
        myInven.Remove(item);

        ReturnSlotToPool(slot.gameObject);

        //판매상태저장
        SaveInventory();
    }

    // 클릭 시 아이템 판매 버튼 팝업창 표시
    public void ShowSellPopup(InventorySlot slot, InventoryItem item)
    {
        popupScript.OpenPopup(slot, item);
    }
}

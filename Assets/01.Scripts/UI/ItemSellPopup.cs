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


    // 팝업에서 처리 중인 슬롯과 아이템 데이터를 임시 저장
    private InventorySlot savedSlot;
    private InventoryItem savedItem;

    /// <summary>
    /// 인벤토리 슬롯 클릭 시 호출되어 팝업을 열고 아이템 정보 세팅
    /// </summary>
    /// <param name="slot"> 아이템이 소속된 인벤토리 슬롯 </param>
    /// <param name="item"> 판매 대상 아이템 정보 </param>
    public void OpenPopup(InventorySlot slot, InventoryItem item)
    {
        // 아이템 데이터 보관
        savedSlot = slot;
        savedItem = item;

        // UI 갱신
        Icon.sprite = item.data.icon;
        nameText.text = item.data.itemName;
        rarityText.text = $"{item.rarity}";
        priceText.text = $"{item.GetSellPrice()}"; // RoundToInt로 반올림 된 가격 적용

        itemSellPopup.SetActive(true);
    }

    // 판매 확인 버튼
    public void OnClickSellBtn()
    {
        InventoryManager.Instance.SellItem(savedSlot,savedItem);

        OnClickCancel();
    }

    // 판매 취소 버튼
    public void OnClickCancel()
    {
        itemSellPopup.SetActive(false);
    }
}

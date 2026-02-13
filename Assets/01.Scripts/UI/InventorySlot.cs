using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [Header("UI")]
    public Image iconImage;
    public Image borderImage;

    private InventoryItem itemInfo;


    /// <summary>
    /// 슬롯에 아이템 데이터를 할당하고 등급에 맞춰 UI 갱신
    /// </summary>
    /// <param name="item"> 인벤토리에 저장된 아이템 데이터 </param>
    public void SetSlot(InventoryItem item)
    {
        itemInfo = item;

        iconImage.sprite = item.data.icon;

        // 등급에 따른 색상 처리
        if (item.rarity == ItemRarity.Legend) borderImage.color = Color.yellow;
        else if (item.rarity == ItemRarity.Epic) borderImage.color = Color.magenta;
        else if (item.rarity == ItemRarity.Rare) borderImage.color = Color.blue;
        else borderImage.color = Color.white;
    }

    // 판매 버튼 클릭시 판매 확인창 호출
    public void OnClickSellButton()
    {
        GameManager.Instance.Inven.ShowSellPopup(this, itemInfo);
    }

}

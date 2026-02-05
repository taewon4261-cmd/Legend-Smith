using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponStoreUI : MonoBehaviour
{

    [Header("Weapon Prefab")]
    public Transform gridContent;
    public GameObject slotPrefab;

    [Header("Manager")]
    public WeaponDBSO weaponDBSO;

    private ItemDataSO currentTarget; // 현재 선택한 무기의 데이터
    public Button unlockBtn;
    public TextMeshProUGUI unlockBtnText;

    [Header("UI")]
    public Image icon;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI costText;

    private void Start()
    {

        //DB에 등록된 모든 무기를 순회하며 상점 슬롯 생성
        foreach (var weapon in weaponDBSO.allWeapon)
        {
            GameObject newWeapon = Instantiate(slotPrefab, gridContent);
            WeaponShopSlot slot = newWeapon.GetComponent<WeaponShopSlot>();

            // 각 슬롯에 데이터와 매니저 참조 전달
            slot.SetWeaponSlot(weapon, this);
        }
    }

    //특정 슬롯 클릭 시 호출되어 정보창에 갱신
   public void SelectedSlot(ItemDataSO item)
    {
        currentTarget = item;

        if(item.icon != null) icon.sprite = item.icon;
        icon.sprite = item.icon;
        nameText.text = item.itemName;
        costText.text = $"{item.cost} G";

        UpdateButtenState();
    }

    // 현재 선택된 아이템의 해금 여부를 확인해서 구매 버튼의 활성화 상태 제어
    void UpdateButtenState()
    {
        if (currentTarget == null) return;

        // PlayerPrefs에 저장된 해금 데이터 확인
        bool isUnlocked = UnlockManager.Instance.CheckUnlock(currentTarget);

        if (isUnlocked)
        {
            unlockBtn.interactable = false;
            unlockBtnText.text = "보유중";
        }
        else
        {
            unlockBtn.interactable = true;
            unlockBtnText.text = "해금";
        }
    }


    // 해금 버튼 클릭 시 실제 구매 프로세스 수행
    public void OnClickPurchaseBtn()
    {
        if (currentTarget == null) return;

        // 중복 구매 방지
        if (UnlockManager.Instance.CheckUnlock(currentTarget)) return;

        //재화 확인 및 차감
        if (ResourceManager.Instance.gold >= currentTarget.unlockedPrice)
        {
            ResourceManager.Instance.AddGold(-currentTarget.unlockedPrice);

            // PlayerPrefs에 저장 및 즉시 반영
            UnlockManager.Instance.PurchaseWeapon(currentTarget);

            UpdateButtenState();
        }
        else
        {
            // TOOD :  돈 부족 팝업 출력 하거나 안하거나
        }

    }

}

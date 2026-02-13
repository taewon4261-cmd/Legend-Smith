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
    public TextMeshProUGUI unlockPriceText;

    private void Start()
    {
        CreateWeaponStoreSlots();

        SelectedSlot(weaponDBSO.allWeapon[0]); // 들어가면 첫번째 무기 선택
    }

    void CreateWeaponStoreSlots()
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
        unlockPriceText.text = $"{item.unlockedPrice} G";

        UpdateButtenState();
    }

    // 현재 선택된 아이템의 해금 여부를 확인해서 구매 버튼의 활성화 상태 제어
    void UpdateButtenState()
    {
        if (currentTarget == null) return;

        // PlayerPrefs에 저장된 해금 데이터 확인
        bool isUnlocked = GameManager.Instance.Unlock.CheckUnlock(currentTarget);

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
        if (GameManager.Instance.Unlock.CheckUnlock(currentTarget)) return;

        //재화 확인 및 차감
        if (GameManager.Instance.Resource.gold >= currentTarget.unlockedPrice)
        {
            GameManager.Instance.Resource.TrySpendGold(currentTarget.unlockedPrice);

            // PlayerPrefs에 저장 및 즉시 반영
            GameManager.Instance.Unlock.PurchaseWeapon(currentTarget);

            UpdateButtenState();

            GameManager.Instance.SFX.PlaySFX("BuySellUpgrade", 1);
        }
        else
        {
            GameManager.Instance.SFX.PlaySFX("OnClickBtnFail", 1);
        }
    }
}

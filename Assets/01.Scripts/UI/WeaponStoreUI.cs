using System.Collections;
using System.Collections.Generic;
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

    private ItemDataSO currentTarget;
    public Button unlockBtn;
    public TextMeshProUGUI unlockBtnText;

    [Header("UI")]
    public Image icon;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI costText;

    private void Start()
    {
        foreach (var weapon in weaponDBSO.allWeapon)
        {
            GameObject newWeapon = Instantiate(slotPrefab, gridContent);

            WeaponShopSlot slot = newWeapon.GetComponent<WeaponShopSlot>();

            slot.SetWeaponSlot(weapon, this);
        }
    }

   public void SelectedSlot(ItemDataSO item)
    {
        currentTarget = item;

        if(item.icon != null) icon.sprite = item.icon;
        icon.sprite = item.icon;
        nameText.text = item.itemName;
        costText.text = $"{item.cost} G";

        UpdateButtenState();
    }

    void UpdateButtenState()
    {
        if (currentTarget == null) return;

        bool isUnlocked = UnlockManager.Instance.CheckUnlock(currentTarget);

        if (isUnlocked)
        {
            unlockBtn.interactable = false;
            unlockBtnText.text = "보유중";
        }
        else
        {
            unlockBtn.interactable=true;
            unlockBtnText.text = "해금";
        }
    }

    public void OnClickPurchaseBtn()
    {
        if (currentTarget == null) return;

        if (UnlockManager.Instance.CheckUnlock(currentTarget)) return;

        if (ResourceManager.Instance.gold >= currentTarget.cost)
        {
            ResourceManager.Instance.AddGold(-currentTarget.cost);

            UnlockManager.Instance.PurchaseWeapon(currentTarget);

            UpdateButtenState();
        }
        else
        {
            // 돈 부족 팝업 출력 하거나 안하거나
        }

    }

}

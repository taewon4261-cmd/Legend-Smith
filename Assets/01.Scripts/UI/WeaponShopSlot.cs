using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponShopSlot : MonoBehaviour
{
    public ItemDataSO data;
    public Image icon;
    private WeaponStoreUI storeUI;

    public bool isUnlocked =false;

    public void SetWeaponSlot(ItemDataSO newData , WeaponStoreUI manager)
    {
        data = newData;
        storeUI = manager;
        icon.sprite = newData.icon;

        SetTransparency(0.5f);

        bool isUnlocked = UnlockManager.Instance.CheckUnlock(data);
    }

    void UpdateColor()
    {
        if (isUnlocked == true)
        {
            icon.color = Color.white;
        }
        else
        {
            icon.color = new Color(1, 1, 1, 0.3f);
        }
    }
    void SetTransparency(float alpha)
    {
        Color color = icon.color;
        color.a = alpha;
        icon.color = color;
    }

    public void OnClickSlot()
    {
        if (storeUI != null)
        {
            storeUI.SelectedSlot(data);
        }
    }
        
}

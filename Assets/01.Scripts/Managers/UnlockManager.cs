using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockManager : MonoBehaviour
{
    public static UnlockManager Instance;

    private void Awake()
    {
        if(Instance == null)Instance = this;
        else Destroy(gameObject);
    }
    
    public bool CheckUnlock(ItemDataSO data)
    {
        if(data.isDefaultUnlocked== true) return true;

        return PlayerPrefs.GetInt("Unlock_" + data.itemName, 0) == 1;
    }

    public void PurchaseWeapon(ItemDataSO data)
    {
        PlayerPrefs.SetInt("Unlock_" + data.itemName, 1);
        PlayerPrefs.Save();
    }

}

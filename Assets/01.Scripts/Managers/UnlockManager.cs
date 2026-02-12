using System.Collections.Generic;
using UnityEngine;

public class UnlockManager : MonoBehaviour
{
    public static UnlockManager Instance;

    public List<ItemDataSO> allWeaponData;

    // 저장시 사용할 키
    private const string UnlockKey = "Unlock_";

    private void Awake()
    {
        if(Instance == null)Instance = this;
        else Destroy(gameObject);
    }
    
    /// <summary>
    /// 아이템의 해금 여부 확인
    /// </summary>
    /// <param name="data"> 아이템 정보 데이터 </param>
    /// <returns>해금되어 있으면 true, 잠겨있으면 false 반환 </returns>
    public bool CheckUnlock(ItemDataSO data)
    {
        //기본적으로 해금된 아이템인 경우 즉시 true
        if(data.isDefaultUnlocked == true) return true;

        // 키값 확인, 1이면 해금 0이면 잠금 판정
        return PlayerPrefs.GetInt(UnlockKey + data.itemName, 0) == 1;
    }

    public int GetUnlockCount()
    {
        int count = 0;
        // 등록된 모든 무기를 하나씩 검사
        foreach (var weapon in allWeaponData)
        {
            if (CheckUnlock(weapon)) // 해금되었는지 확인
            {
                count++;
            }
        }
        return count;
    }

    /// <summary>
    /// 아이템 구매시 데이터를 PlayerPrefs로 저장하는 함수
    /// </summary>
    /// <param name="data">구매한 아이템 정보</param>
    // 현재까지 해금한 무기 중 가장 높은 가격(티어)을 반환
    public int GetHighestWeaponCost()
    {
        return PlayerPrefs.GetInt("MaxWeaponCost", 0);
    }

    public void PurchaseWeapon(ItemDataSO data)
    {
        PlayerPrefs.SetInt(UnlockKey + data.itemName, 1);

        int currentMax = PlayerPrefs.GetInt("MaxWeaponCost", 0);
        if (data.unlockedPrice > currentMax) PlayerPrefs.SetInt("MaxWeaponCost", data.unlockedPrice);

        PlayerPrefs.Save();

        int count = GetUnlockCount();

        LootLockerManager.Instance.SubmitScore("rank_weapon_tier", count);
    }


}

using System.Collections.Generic;
using UnityEngine;

public class UnlockManager : MonoBehaviour
{
    public List<ItemDataSO> allWeaponData;

    // 저장시 사용할 키
    private const string UnlockKey = "Unlock_";

    // 검색 최적화를 위한 딕셔너리 캐싱
    private Dictionary<string, bool> unlockCache = new Dictionary<string, bool>();

    public void Init()
    {
        unlockCache.Clear();

        foreach (var data in allWeaponData)
        {
            bool isUnlocked = data.isDefaultUnlocked;

            if (!isUnlocked)
            {
                if (PlayerPrefs.GetInt(UnlockKey + data.itemName, 0) == 1)
                {
                    isUnlocked = true;
                }
            }
            unlockCache.Add(data.itemName, isUnlocked);
        }
    }

    /// <summary>
    /// 아이템의 해금 여부 확인
    /// </summary>
    /// <param name="data"> 아이템 정보 데이터 </param>
    /// <returns>해금되어 있으면 true, 잠겨있으면 false 반환 </returns>
    public bool CheckUnlock(ItemDataSO data)
    {
        if (unlockCache.TryGetValue(data.itemName, out bool isUnlocked))
        {
            return isUnlocked;
        }
        return false;
    }

    public int GetUnlockCount()
    {
        int count = 0;
       
        foreach (var weapon in allWeaponData)  // 등록된 모든 무기를 하나씩 검사
        {
            if (CheckUnlock(weapon)) count++; // 해금되었는지 확인
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

        // 딕셔너리 런타임 갱신
        unlockCache[data.itemName] = true;

        // 랭킹 등록용
        int currentMax = PlayerPrefs.GetInt("MaxWeaponCost", 0);
        if (data.unlockedPrice > currentMax)
        {
            PlayerPrefs.SetInt("MaxWeaponCost", data.unlockedPrice);
        }

        PlayerPrefs.Save();

        int count = GetUnlockCount();
        if (GameManager.Instance != null)
        {
            GameManager.Instance.LootLocker.SubmitScore("rank_weapon_tier", count);
        }
    }


}

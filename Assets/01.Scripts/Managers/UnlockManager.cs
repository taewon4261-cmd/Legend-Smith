using System.Collections.Generic;
using UnityEngine;

public class UnlockManager : MonoBehaviour
{
    public List<ItemDataSO> allWeaponData;

    // 저장시 사용할 키
    private const string UnlockKey = "Unlock_";

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
        // 1. 저장소(PlayerPrefs)에 저장 (영구 보관)
        PlayerPrefs.SetInt(UnlockKey + data.itemName, 1);

        // 2. 목록표(Dictionary)도 업데이트! (중요: 이걸 해야 재시작 안 해도 바로 반영됨)
        if (unlockCache.ContainsKey(data.itemName))
        {
            unlockCache[data.itemName] = true;
        }
        else
        {
            unlockCache.Add(data.itemName, true);
        }

        // 최대 가격 갱신 로직
        int currentMax = PlayerPrefs.GetInt("MaxWeaponCost", 0);
        if (data.unlockedPrice > currentMax)
        {
            PlayerPrefs.SetInt("MaxWeaponCost", data.unlockedPrice);
        }
        PlayerPrefs.Save();

        // 랭킹 등록
        int count = GetUnlockCount();
        if (GameManager.Instance != null)
        {
            GameManager.Instance.LootLocker.SubmitScore("rank_weapon_tier", count);
        }
    }


}

using UnityEngine;

public class UnlockManager : MonoBehaviour
{
    public static UnlockManager Instance;

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
    /// <summary>
    /// 아이템 구매시 데이터를 PlayerPrefs로 저장하는 함수
    /// </summary>
    /// <param name="data">구매한 아이템 정보</param>
    // 현재까지 해금한 무기 중 가장 높은 가격(티어)을 반환
    public int GetHighestWeaponCost()
    {
        // PurchaseWeapon 호출 시마다 PlayerPrefs에 "MaxWeaponCost"를 저장하도록 PurchaseWeapon을 수정하거나
        // 여기서 모든 아이템을 체크해야 합니다. 
        // 우선은 PlayerPrefs에 저장된 값을 가져오는 방식이 가장 효율적입니다.
        return PlayerPrefs.GetInt("MaxWeaponCost", 0);
    }

    // PurchaseWeapon 함수 안에도 아래 한 줄을 추가해두면 좋습니다.
    public void PurchaseWeapon(ItemDataSO data)
    {
        PlayerPrefs.SetInt(UnlockKey + data.itemName, 1);

        // 최고 기록 갱신용 저장
        int currentMax = PlayerPrefs.GetInt("MaxWeaponCost", 0);
        if (data.cost > currentMax) PlayerPrefs.SetInt("MaxWeaponCost", data.cost);

        PlayerPrefs.Save();
        LootLockerManager.Instance.SubmitScore("rank_weapon_tier", data.cost);
    }


}
